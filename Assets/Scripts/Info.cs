using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    UIManager m_uiManager;
    [SerializeField] string m_title;
    [SerializeField] [TextArea] string m_info;

    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_uiManager.ShowInfo(m_title, m_info);
        }
    }
}
