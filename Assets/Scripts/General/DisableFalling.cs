using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFalling : MonoBehaviour
{
    [SerializeField] bool disable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (disable) 
                other.transform.parent.GetComponent<PlayerScript>().FallingDisabled = true;
            else if(!disable)
            {
                other.transform.parent.GetComponent<PlayerScript>().FallingDisabled = false;
            }
        }

    }
}
