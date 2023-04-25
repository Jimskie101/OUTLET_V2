using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EasyButtons;

public class Roomba : MonoBehaviour
{
    NavMeshAgent m_agent;
    Transform m_playerParent;

    [SerializeField] Vector3 m_targetLocation;
    
    private void OnEnable() {
        m_agent = GetComponent<NavMeshAgent>();
    }
    
    [Button]
    private void GoToTarget()
    {
        m_agent.SetDestination(m_targetLocation);
    }
    private void StopMoving()
    {
        m_agent.isStopped = true;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {   
            m_playerParent = other.transform.parent;
            other.transform.SetParent(this.transform);
            if( m_agent.isStopped)  m_agent.isStopped = false;
            GoToTarget();
        }
    }
    private void OnTriggerExit(Collider other) {
         if(other.CompareTag("Player"))
        {
            other.transform.SetParent(m_playerParent);
            StopMoving();
        }
    }

}
