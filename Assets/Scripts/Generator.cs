using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Generator : MonoBehaviour
{
    CutsceneManager m_cutsceneManager;

    private void Start()
    {
        m_cutsceneManager = Managers.Instance.CutsceneManager;
    }


    [Header("Mover")]
    [SerializeField] bool m_moveParent;
    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_moveDuration = 0;

    Rigidbody m_targetRB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            if (m_moveParent)
            {
                MoveTargetObject(other.transform.parent);
            }
            else MoveTargetObject(other.transform);
        }
    }

    [SerializeField] PuzzleIndicator m_puzzleIndicator;

    public void MoveTargetObject(Transform targetObj)
    {
        m_defaultPosition = targetObj.localPosition;
        if (targetObj.TryGetComponent(out m_targetRB))
        {
            m_targetRB.isKinematic = true;
        }
        targetObj.DOLocalMove(m_defaultPosition - m_targetPosition, m_moveDuration)
        .OnComplete(() => {m_puzzleIndicator.Activate(); Fixed();});
    }

    [SerializeField] string m_workingSoundName;

    [SerializeField] Light[] m_targetLights;
    [SerializeField] GameObject[] m_targetObjects;
    [Button]
    public void Fixed()
    {
        //Managers.Instance.AudioManager.PlayHere(m_workingSoundName, this.gameObject);
        StartCoroutine(LightUp());
        //Managers.Instance.CutsceneManager.PlayTimeline();
        //GetComponent<Outline>().enabled = false;

    }


    IEnumerator LightUp()
    {
        yield return new WaitForSeconds(2f);

        m_cutsceneManager.PlayCutscene(0);
        foreach (GameObject g in m_targetObjects)
            g.SetActive(true);

        foreach (Light l in m_targetLights)
            l.DOIntensity(4, 1f).SetEase(Ease.InOutBounce);

    }


}
