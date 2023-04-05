using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerToEvent : MonoBehaviour
{
    [SerializeField] string m_targetTag = "PuzzlePiece";
    [SerializeField] UnityEvent m_action;
    [SerializeField] float m_delay = 0;
    WaitForSeconds m_delayTime;
    private void Start()
    {
        m_delayTime = new WaitForSeconds(m_delay);
    }
    [SerializeField] int m_callsneeded = 2;
    [SerializeField] int m_callCount = 0;
    public void Called()
    {
        m_callCount++;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_targetTag))
        {
            if (m_callsneeded <= m_callCount)
            {
                StartCoroutine(InvokeEvent());
            }

        }
    }

    IEnumerator InvokeEvent()
    {
        yield return m_delayTime;
        m_action.Invoke();
    }
}
