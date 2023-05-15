using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redamageplayer : MonoBehaviour
{bool isdone = false;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
        if (!isdone){ isdone=true;
other.gameObject.GetComponent<PlayerScript>().LifePercentage = 
other.gameObject.GetComponent<PlayerScript>().LifePercentage * 0.5f;

this.gameObject.SetActive(false);
            }
    }}
}