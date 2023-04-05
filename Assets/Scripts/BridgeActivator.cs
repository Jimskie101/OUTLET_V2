using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BridgeActivator : MonoBehaviour
{
    //How many collision needed to activate the bridge
    [SerializeField] int m_collisionNeeded = 8;
    [SerializeField] int m_collisionCounter = 0;

    //Invocable Unity Events;
    [SerializeField] UnityEvent m_ActivateEvent;
    [SerializeField] UnityEvent m_DeactivateEvent;

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            m_collisionCounter--;
            m_DeactivateEvent.Invoke();
            
        }
    }




}
