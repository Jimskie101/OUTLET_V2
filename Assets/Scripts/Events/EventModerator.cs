using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventModerator : MonoBehaviour
{
    [SerializeField] int m_callsNeeded = 2;
    [SerializeField] int m_callCount = 0;
    [SerializeField] UnityEvent m_ActivateEvent;
    

    public void Called()
    {
        m_callCount++;
        if(m_callsNeeded <= m_callCount)
        {
            m_ActivateEvent.Invoke();
        }
    }
   
}
