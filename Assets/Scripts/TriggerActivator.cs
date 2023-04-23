using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class TriggerActivator : MonoBehaviour
{
    //How many collision needed to activate the bridge
    [SerializeField] int m_collisionNeeded = 8;
    [SerializeField] int m_collisionCounter = 0;
    [SerializeField] float m_delayEventTime = 0;

    //Invocable Unity Events;
    [SerializeField] UnityEvent m_ActivateEvent;
    [SerializeField] UnityEvent m_DeactivateEvent;
    [SerializeField] bool m_runOnlyOnce = false;
    bool m_objectiveDone = false;

    private void Start()
    {
        Invoke("ObjectiveChecker", 0.1f);
    }
    Engaged m_engaged;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_objectiveDone)
        {
            if (other.CompareTag("PuzzlePiece"))
            {
                m_collisionCounter++;
               
                if (m_collisionNeeded <= m_collisionCounter)
                {
                     m_objectiveDone = true;
                    if (m_collisionCounter % 4 == 0)
                    {
                        if (other.transform.parent.TryGetComponent(out m_engaged))
                        {
                            m_engaged.Engage();
                        }
                    }
                    DOVirtual.DelayedCall(m_delayEventTime, () =>
                    {
                        m_ActivateEvent.Invoke();
                        
                        Debug.Log("Objective Done");
                        if (m_runOnlyOnce)
                        {
                            return;
                        }


                    });

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

    public void SetObjective() { Managers.Instance.GameManager.ObjectiveCounter = m_forLoadingId; }

    [Header("ForLoading")]
    [SerializeField] int m_forLoadingId;

    [SerializeField] UnityEvent m_forLoadingEvents;
    //Must have for objectives

    private void ForLoading()
    {
        m_objectiveDone = true;
        Debug.Log("Forloading", this.gameObject);
        m_forLoadingEvents.Invoke();



    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
    }


}
