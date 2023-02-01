using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Info : MonoBehaviour
{
    UIManager m_uiManager;
    [SerializeField] string m_title;
    [SerializeField][TextArea] string m_info;
    Vector3 m_rotation = new Vector3(0, 360f, 0);

    [Header("Control Hint")]
    [SerializeField] bool m_isAControlHint = false;
    [SerializeField] GameObject m_uiWithHint;
    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (m_isAControlHint) 
            { 
                m_uiManager.ShowInfo("","",true,m_uiWithHint);
            }
            else
            {
                m_uiManager.ShowInfo(m_title, m_info);
            }

        }
    }
}
