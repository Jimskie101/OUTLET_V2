using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Cinemachine;

public class Tools : MonoBehaviour
{

    public static Tools Instance { get;  set; }
    public bool IsAndroid = false;
    [SerializeField] GameObject m_androidUI;




    private void OnEnable()
    {
        if (!IsAndroid)
        {
            m_androidUI.SetActive(false);
        }
    }



    [Header("Test Methods")]
    [SerializeField] GameObject dummyGameObject;
    
}
