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

        m_reEnablerDelay = new WaitForSeconds(4f);
    }
    private void OnEnable()
    {
        m_particles = null;
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        transform.DOLocalMoveY(transform.localPosition.y + 0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
    MeshRenderer m_mesh;
    BoxCollider m_boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            Managers.Instance.AudioManager.PlayHere("collect", other.gameObject, false, true);
            if (m_isAControlHint)
            {
                m_uiManager.ShowInfo("", "", true, m_uiWithHint);
            }
            else
            {
                m_uiManager.ShowInfo(m_title, m_info);
            }
            if (TryGetComponent<MeshRenderer>(out m_mesh))
                m_mesh.enabled = false;
            if (TryGetComponent<BoxCollider>(out m_boxCollider))
                m_boxCollider.enabled = false;

            StartCoroutine(ReEnable());



        }
    }

    ParticleSystem m_particles = null;
    IEnumerator ReEnable()
    {   
        transform.GetComponentInChildren<ParticleSystem>();
        if(m_particles != null)
        m_particles.Play();
        yield return m_reEnablerDelay;
        if (TryGetComponent<MeshRenderer>(out m_mesh))
            m_mesh.enabled = true;
        if (TryGetComponent<BoxCollider>(out m_boxCollider))
            m_boxCollider.enabled = true;

    }
}
