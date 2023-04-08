using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerActivator : MonoBehaviour
{
    //How many collision needed to activate the bridge
    [SerializeField] int m_collisionNeeded = 8;
    [SerializeField] int m_collisionCounter = 0;

    //Invocable Unity Events;
    [SerializeField] UnityEvent m_ActivateEvent;
    [SerializeField] UnityEvent m_DeactivateEvent;

    bool m_objectiveDone = false;

    private void Start()
    {
        Invoke("ObjectiveChecker", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_objectiveDone)
        {
            if (other.CompareTag("PuzzlePiece"))
            {
                m_collisionCounter++;
                if (m_collisionNeeded <= m_collisionCounter)
                {

                    m_ActivateEvent.Invoke();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_objectiveDone)
        {
            if (other.CompareTag("PuzzlePiece"))
            {
                m_collisionCounter--;
                m_DeactivateEvent.Invoke();

            }
        }
    }

    public void SetObjective() {Managers.Instance.GameManager.ObjectiveCounter = m_forLoadingId;}
    
    [Header("ForLoading")]
    [SerializeField] int m_forLoadingId;

    [SerializeField] UnityEvent m_forLoadingEvents;
    //Must have for objectives

    private void ForLoading()
    {
        m_objectiveDone = true;
        Debug.Log("Forloading");
        m_forLoadingEvents.Invoke();



    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
    }


}
