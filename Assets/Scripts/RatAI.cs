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
    private void Start()
    {
        m_cdTime = new WaitForSeconds(m_attackCD);
    }

    IEnumerator Cooldown()
    {
        yield return m_cdTime;
        m_alreadyAttacked = false;
    }



    private void Update()
    {
        //if in attack range, attack player
        if (Vector3.Distance(transform.position, m_player.position) < m_attackRange)
        {
            
            transform.LookAt(m_player);
            if (!m_alreadyAttacked)
            {
                Debug.Log("attacked");
                m_alreadyAttacked = true;
                
                m_agent.velocity += transform.forward * m_attackForce;
                
                StartCoroutine(Cooldown());

            }
        }



        //if in chase range, chase player
        else if (Vector3.Distance(transform.position, m_player.position) < m_chaseRange)
        {
            m_agent.isStopped = false;
            m_onChase = true;
            m_target = m_player.position;
            m_agent.SetDestination(m_player.position);

        }

        else if (!m_agent.pathPending)
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
