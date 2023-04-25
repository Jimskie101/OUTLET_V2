using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private PlayerScript m_playerScript;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Short Circuit");
            m_playerScript = other.transform.GetComponentInParent<PlayerScript>();
            
            m_playerScript.IsDead = true;
            
        }
    }
}
