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

    WaitForSeconds m_reEnablerDelay;

    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;

        m_reEnablerDelay = new WaitForSeconds(2f);
    }
    private void OnEnable()
    {
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        transform.DOLocalMoveY(transform.localPosition.y + 0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (m_isAControlHint)
            {
                m_uiManager.ShowInfo("", "", true, m_uiWithHint);
            }
            else
            {
                m_uiManager.ShowInfo(m_title, m_info);
            }
            foreach (Behaviour component in GetComponents<Behaviour>())
            {
                component.enabled = false;
            }
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ReEnable());



        }
    }


    IEnumerator ReEnable()
    {

        yield return m_reEnablerDelay;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;

    }
}
