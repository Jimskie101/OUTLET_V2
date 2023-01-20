using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{

    public static Tools Instance { get; private set; }
    public bool IsAndroid = false;
    [SerializeField] GameObject m_androidUI;




    private void OnEnable()
    {
        if(!IsAndroid)
        {
            m_androidUI.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
