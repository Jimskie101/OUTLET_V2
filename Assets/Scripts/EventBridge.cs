using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBridge : MonoBehaviour
{
    [SerializeField] UnityEvent m_ActivateEvent;
    [SerializeField] UnityEvent m_DeactivateEvent;


    [SerializeField] int m_callsCount = 0;
    [SerializeField] int m_calls = 0;



    

    public void Activate()
    {
        m_ActivateEvent.Invoke();
    }
    public void Deactivate()
    {
        m_DeactivateEvent.Invoke();
    }
}
