using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TerminalEvent : MonoBehaviour
{
    [SerializeField] UnityEvent m_Action;
    bool m_objectiveDone = false;
    
    public void TerminalConnected()
    {
        if (!m_objectiveDone)
        {
            Debug.Log("Terminal Connected");
            m_objectiveDone = true;
            DOVirtual.DelayedCall(1f, () => m_Action.Invoke()) ;
        }

    }

    BoxCollider[] m_colliders;

    private void Start()
    {
        m_colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider b in m_colliders)
        {
            b.enabled = false;
        }
        Invoke("ObjectiveChecker", 0.1f);
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
        else
        {
            foreach (BoxCollider b in m_colliders)
            {
                b.enabled = true;
            }
        }
    }

}
