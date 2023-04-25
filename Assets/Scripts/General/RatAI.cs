using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EasyButtons;

public class RatAI : MonoBehaviour
{
    [SerializeField] RatData m_ratData;
    [Space]
    [SerializeField] Color m_statusIndicator = Color.white;
#pragma warning disable
    [SerializeField] string m_status;
#pragma warning restore

    [Space]
    [SerializeField] Transform m_player;
    NavMeshAgent m_agent;
    Animator m_ratAnimator;
    Vector3 m_nextPos;
    Vector3 m_target;
    [SerializeField] bool m_alreadyAttacked = false;
    bool m_alreadyDamaged = false;
    WaitForSeconds m_cdTime;
    WaitForSeconds m_damageTime;
    float m_targetDistance;

    Vector3 m_lookPos;
    Quaternion m_rotation;
    [SerializeField] float angleToLook = 0.01f;

    AudioSource m_audioSrc;



    private void SetRatValues()
    {
        m_agent.speed = m_ratData.Speed;
        m_agent.angularSpeed = m_ratData.AngularSpeed;
        m_agent.acceleration = m_ratData.Acceleration;
    }




    private void OnEnable()
    {
        m_cdTime = new WaitForSeconds(m_ratData.AttackCooldown);
        m_damageTime = new WaitForSeconds(m_ratData.DamageResetTime);
        m_agent = GetComponent<NavMeshAgent>();
        m_ratAnimator = GetComponentInChildren<Animator>();
        m_playerCc = m_player.GetComponent<CharacterController>();
        m_playerScript = m_player.GetComponent<PlayerScript>();
        SetRatValues();

    }


    private bool FaceTarget(Vector3 pos)
    {
        m_lookPos = pos - transform.position;
        m_lookPos.y = 0;
        m_rotation = Quaternion.LookRotation(m_lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_rotation, Time.deltaTime * 5);
        if (Quaternion.Angle(transform.rotation, m_rotation) <= angleToLook)
            return true;
        else return false;
    }

    CharacterController m_playerCc;
    PlayerScript m_playerScript;
    [SerializeField] bool m_doneAttacking = false;
    bool m_isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (!m_alreadyDamaged)
            {
                Debug.Log("PlayerDamaged");
                m_alreadyDamaged = true;
                m_playerScript.TakeDamage(m_ratData.AttackDamage);
                StartCoroutine(DamageCooldown());
            }


        }
    }


    private void Update()
    {



        m_isMoving = m_agent.velocity.sqrMagnitude == 0f ? false : true;
        m_ratAnimator.SetBool("isMoving", m_isMoving);


        m_targetDistance = Vector3.Distance(transform.position, m_player.position);
        //return to attacking position
        if (m_agent.velocity == Vector3.zero && m_doneAttacking)
        {

            m_agent.isStopped = false;
            m_doneAttacking = false;
            if (m_oldPosition != Vector3.zero)
                m_agent.SetDestination(m_oldPosition);
        }


        // // Check if the agent is close to the character
        // if (m_targetDistance < m_agent.radius + m_playerCc.radius)
        // {
        //     if (!m_alreadyDamaged)
        //     {
        //         Debug.Log("PlayerDamaged");
        //         m_alreadyDamaged = true;
        //         m_playerScript.TakeDamage(m_ratData.AttackDamage);
        //         StartCoroutine(DamageCooldown());
        //     }

        //     // Calculate push direction
        //     Vector3 pushDirection = (m_playerCc.transform.position - m_agent.transform.position).normalized;

        //     // Apply push force to the character controller
        //     m_playerCc.Move(pushDirection * (m_ratData.AttackForce * .5f) * Time.deltaTime);
        // }

        //if in chase range, chase player
        if (m_targetDistance <= m_ratData.ChaseRange && m_targetDistance > m_ratData.AttackRange)
        {
            m_agent.isStopped = false;
            // if (!m_isMoving) { m_isMoving = true; m_ratAnimator.SetBool("isMoving", m_isMoving); }
            m_status = " Chasing";
            m_statusIndicator = Color.green;
            m_target = m_player.position;
            m_agent.SetDestination(m_player.position);


        }
        else if (m_targetDistance <= m_ratData.AttackRange)
        {
            if (m_agent.enabled) m_agent.isStopped = true;
            FaceTarget(m_player.position);

            if (!m_alreadyAttacked)
            {
                m_alreadyAttacked = true;
                AttackPlayer();
                StartCoroutine(Cooldown());
            }
            else
            {
                // if (m_isMoving) { m_isMoving = false; m_ratAnimator.SetBool("isMoving", m_isMoving); }
            }

        }

        else if (m_targetDistance < m_ratData.ActivityRange)
        {
            if (m_audioSrc == null)
                m_audioSrc = Managers.Instance.AudioManager.PlayHere("rat", this.gameObject, true);
            else m_audioSrc.Play();

            if (!m_isMoving)
            {
                m_isMoving = true;
                // m_ratAnimator.SetBool("isMoving", m_isMoving);

            }
            m_agent.isStopped = false;
            if (!m_agent.pathPending)
            {
                if (m_agent.remainingDistance <= m_agent.stoppingDistance)
                {
                    if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)
                    {

                        RandomMove();


                    }
                }
            }

        }
        else
        {
            if (m_audioSrc != null)
                m_audioSrc.Stop();


            m_agent.isStopped = true;
            m_status = " Neutral";
            m_statusIndicator = Color.white;
            m_isMoving = false;
            // m_ratAnimator.SetBool("isMoving", m_isMoving);
        }
    }

    IEnumerator Cooldown()
    {

        yield return m_cdTime;
        m_alreadyAttacked = false;
    }

    IEnumerator DamageCooldown()
    {
        yield return m_damageTime;
        m_alreadyDamaged = false;
    }

    Vector3 m_newVelocity;
    Vector3 m_oldPosition = Vector3.zero;
    [Button]
    private void AttackPlayer()
    {
        while (!m_doneAttacking)
        {
            if (FaceTarget(m_player.position))
            {
                m_status = " Attacking";
                m_statusIndicator = Color.red;
                m_oldPosition = transform.position;
                m_agent.isStopped = false;
                m_newVelocity = Vector3.zero;
                m_newVelocity = transform.forward * m_ratData.AttackForce;
                m_ratAnimator.SetTrigger("attack");
                m_agent.velocity = m_newVelocity;
                m_doneAttacking = true;
            }
        }

    }







    [Button]
    private void RandomMove()
    {
        // if (!m_isMoving) { m_isMoving = true; m_ratAnimator.SetBool("isMoving", m_isMoving); }
        m_status = " Wandering";
        m_statusIndicator = Color.yellow;
        m_nextPos = Random.insideUnitSphere * Random.Range(m_ratData.WanderRadius.x, m_ratData.WanderRadius.y);
        m_target = m_nextPos + transform.position;
        m_agent.SetDestination(m_target);

    }
   


}

