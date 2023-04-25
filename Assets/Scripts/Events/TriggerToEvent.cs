using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TriggerToEvent : MonoBehaviour
{
    [SerializeField] string m_targetTag = "PuzzlePiece";
    [SerializeField] UnityEvent m_indicatorEvent;
    [SerializeField] UnityEvent m_action;
    [SerializeField] float m_delayTime = 0;
    [SerializeField] bool m_triggerCountAsCall = false;
    bool m_objectiveDone = false;
    BoxCollider [] m_colliders;

    private void Start()
    {
        m_colliders = GetComponents<BoxCollider>();
        foreach(BoxCollider b in m_colliders)
        {
            b.enabled = false;
        }
        Invoke("ObjectiveChecker", 0.1f);
    }

    [SerializeField] int m_callsNeeded = 2;
    [SerializeField] int m_callCount = 0;
    public void Called()
    {
        m_callCount++;
        if (m_callsNeeded <= m_callCount)
            m_indicatorEvent.Invoke();
    }
    public void UnCalled()
    {
        if (m_callCount > 0)
            m_callCount--;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!m_objectiveDone)
        {
            if (other.CompareTag(m_targetTag))
            {
                if (m_triggerCountAsCall) Called();

                if (m_callsNeeded <= m_callCount)
                {
                    DOVirtual.DelayedCall(m_delayTime, () => m_action.Invoke());
                }


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
        Debug.Log("Forloading",this.gameObject);
        m_forLoadingEvents.Invoke();



    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
        else
        {
             foreach(BoxCollider b in m_colliders)
        {
            b.enabled = true;
        }
        }
    }

}
