using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour
{
    private void Start() {
        Managers.Instance.UIManager.ShowGameUpdate("Level Started");
    }
     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          
        }
        
    }

}
