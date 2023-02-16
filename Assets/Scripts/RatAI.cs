using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EasyButtons;

public class RatAI : MonoBehaviour
{
    [Space]
    [SerializeField] Color m_statusIndicator = Color.white;
#pragma warning disable
    [SerializeField] string m_status;
#pragma warning restore

    [Space]
    [SerializeField] Transform m_player;
    NavMeshAgent m_agent;
    Animator m_ratAnimator;
    [SerializeField] Vector2 wanderRadius;
    [SerializeField] float m_attackRange;
    [SerializeField] float m_chaseRange;
    [SerializeField] float m_activityRange;
    [SerializeField] float m_attackForce;
    [SerializeField] float m_attackCD;
    [SerializeField] float m_attackDamage;
    [SerializeField] float m_damageCD;
    Vector3 m_nextPos;
    Vector3 m_target;
    [SerializeField] bool m_alreadyAttacked = false;
    bool m_alreadyDamaged = false;
    WaitForSeconds m_cdTime;
    WaitForSeconds m_damageTime;
    float m_targetDistance;

    Vector3 m_lookPos;
    Quaternion m_rotation;
    [SerializeField] float angleToShoot = 0.01f;









    private void OnEnable()
    {
        m_cdTime = new WaitForSeconds(m_attackCD);
        m_damageTime = new WaitForSeconds(m_damageCD);
        m_agent = GetComponent<NavMeshAgent>();
        m_ratAnimator = GetComponentInChildren<Animator>();
        m_playerCc = m_player.GetComponent<CharacterController>();
        m_playerScript = m_player.GetComponent<PlayerScript>();
    }



    private bool FaceTarget(Vector3 pos)
    {
        m_lookPos = pos - transform.position;
        m_lookPos.y = 0;
        m_rotation = Quaternion.LookRotation(m_lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_rotation, Time.deltaTime * 5);
        if (Quaternion.Angle(transform.rotation, m_rotation) <= angleToShoot)
            return true;
        else return false;
    }

    CharacterController m_playerCc;
    PlayerScript m_playerScript;
    [SerializeField] bool m_doneAttacking = false;
    bool m_isMoving = false;
    private void Update()
    {
        m_isMoving = m_agent.velocity.sqrMagnitude == 0f ? false : true;
        m_ratAnimator.SetBool("isMoving", m_isMoving);


        m_targetDistance = Vector3.Distance(transform.position, m_player.position);

        if (m_agent.velocity == Vector3.zero && m_doneAttacking)
        {

            m_agent.isStopped = false;
            m_doneAttacking = false;
            //m_agent.SetDestination(m_oldPosition);
        }


        // Check if the agent is close to the character
        if (m_targetDistance < m_agent.radius + m_playerCc.radius)
        {
            if (!m_alreadyDamaged)
            {
                Debug.Log("PlayerDamaged");
                m_alreadyDamaged = true;
                m_playerScript.LifePercentage -= m_attackDamage;
                StartCoroutine(DamageCooldown());
            }

            // Calculate push direction
            Vector3 pushDirection = (m_playerCc.transform.position - m_agent.transform.position).normalized;

            // Apply push force to the character controller
            m_playerCc.Move(pushDirection * (m_attackForce * .5f) * Time.deltaTime);
        }

        //if in chase range, chase player
        if (m_targetDistance <= m_chaseRange && m_targetDistance > m_attackRange)
        {
            m_agent.isStopped = false;
            // if (!m_isMoving) { m_isMoving = true; m_ratAnimator.SetBool("isMoving", m_isMoving); }
            m_status = " Chasing";
            m_statusIndicator = Color.green;
            m_target = m_player.position;
            m_agent.SetDestination(m_player.position);


        }
        else if (m_targetDistance <= m_attackRange)
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

        else if (m_targetDistance < m_activityRange)
        {
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
        m_status = " Attacking";
        m_statusIndicator = Color.red;
        m_oldPosition = transform.position;
        m_agent.isStopped = false;
        m_newVelocity = Vector3.zero;
        m_newVelocity = transform.forward * m_attackForce;
        m_ratAnimator.SetTrigger("attack");
        m_agent.velocity = m_newVelocity;
        m_doneAttacking = true;
    }







    [Button]
    private void RandomMove()
    {
        // if (!m_isMoving) { m_isMoving = true; m_ratAnimator.SetBool("isMoving", m_isMoving); }
        m_status = " Wandering";
        m_statusIndicator = Color.yellow;
        m_nextPos = Random.insideUnitSphere * Random.Range(wanderRadius.x, wanderRadius.y);
        m_target = m_nextPos + transform.position;
        m_agent.SetDestination(m_target);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, m_target);
    }



}

