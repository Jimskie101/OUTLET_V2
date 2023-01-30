using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            if (!other.gameObject.activeSelf)
            {
                
                other.gameObject.SetActive(true);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            if (other.gameObject.activeSelf)
            {
                Debug.Log(other.name);
                other.gameObject.SetActive(false);
            }
        }
    }

}
