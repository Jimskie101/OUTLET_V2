using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EasyButtons;

public class RatAI : MonoBehaviour
{
    [SerializeField] Transform m_player;
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] Rigidbody m_rb;
    [SerializeField] Vector2 wanderRadius;
    [SerializeField] float m_attackRange;
    [SerializeField] float m_chaseRange;
    [SerializeField] float m_attackForce;
    [SerializeField] float m_attackCD;
    Vector3 m_nextPos;
    Vector3 m_target;
    [SerializeField] bool m_onChase = false;
    [SerializeField] bool m_alreadyAttacked = false;
    WaitForSeconds m_cdTime;
    float m_targetDistance;

    Vector3 m_lookPos;
    Quaternion m_rotation;
    [SerializeField] float angleToShoot = 0.01f;

    private void OnEnable()
    {
        m_cdTime = new WaitForSeconds(m_attackCD);
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

    private void Update()
    {   
        m_targetDistance = Vector3.Distance(transform.position, m_player.position);


        //if in chase range, chase player
        if (m_targetDistance <= m_chaseRange && m_targetDistance > m_attackRange)
        {
            m_agent.isStopped = false;
            m_onChase = true;
            m_target = m_player.position;
            m_agent.SetDestination(m_player.position);

        }
        else if (m_targetDistance <= m_attackRange)
        {
            if(m_agent.enabled)m_agent.isStopped = true;
            FaceTarget(m_player.position);
           
        }

        else
        {
            m_agent.isStopped = false;
            if (!m_agent.pathPending)
            {
                if (m_agent.remainingDistance <= m_agent.stoppingDistance)
                {
                    if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)
                    {

                        m_onChase = false;
                        RandomMove();


                    }
                }
            }

        }
    }

    IEnumerator Cooldown()
    {
        yield return m_cdTime;
        m_alreadyAttacked = false;
    }

    [Button]
    private void AttackPlayer()
    {   
        m_agent.enabled = false;
        m_rb.isKinematic = false;
        m_rb.AddForce(transform.forward * m_attackForce, ForceMode.Impulse);
        
        StartCoroutine(DashAttack());
    }

    IEnumerator DashAttack()
    {
        yield return m_cdTime;
        m_rb.AddForce(-transform.forward * m_attackForce, ForceMode.Impulse);
        
        StartCoroutine(WindUp());
    }

    IEnumerator WindUp()
    {
        yield return m_cdTime;
        m_rb.isKinematic = true;
        m_agent.enabled = true;
        
        
    }


    [Button]
    private void RandomMove()
    {
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
