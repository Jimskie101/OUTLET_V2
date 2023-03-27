using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using EasyButtons;

public class ObjectiveEvents : MonoBehaviour
{
    CutsceneManager m_cutsceneManager;

    private void Start()
    {
        m_cutsceneManager = Managers.Instance.CutsceneManager;
        Invoke("ObjectiveChecker", 0.1f);
    }


    [Header("Mover")]
    [SerializeField] bool m_moveParent;
    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_moveDuration = 0;
    [SerializeField] int m_collisionNeeded = 1;

    Rigidbody m_targetRB;
    bool m_onGoing = false;
    [SerializeField] int m_collisionCounter = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece") && !m_onGoing)
        {
            m_collisionCounter++;
            if (m_collisionNeeded <= m_collisionCounter)
            {
                if (m_moveParent)
                {
                    MoveTargetObject(other.transform.parent);
                }
                else MoveTargetObject(other.transform);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            m_collisionCounter--;

        }
    }


    public void MoveTargetObject(Transform targetObj)
    {
        Managers.Instance.GameManager.Player.GetComponent<LockAndPullObject>().UnlockObject();
        targetObj.tag = "Environment";
        m_onGoing = true;
        m_defaultPosition = targetObj.localPosition;
        if (targetObj.TryGetComponent(out m_targetRB))
        {
            m_targetRB.isKinematic = true;
        }
        targetObj.DOLocalMove(m_defaultPosition - m_targetPosition, m_moveDuration)
        .OnComplete(() =>
        {
            //m_puzzleIndicator.Activate();
            Fixed();
            m_onGoing = false;
        });
    }

    [SerializeField] string m_workingSoundName;
    bool m_objectiveDone = false;
    [Button]
    public void Fixed()
    {
        //Update the GameManager objectives
        Managers.Instance.GameManager.ObjectiveCounter = m_forLoadingId;


        Managers.Instance.AudioManager.PlayHere(m_workingSoundName, this.gameObject, true);
        if(!m_objectiveDone)
        StartCoroutine(Activate());
        //Managers.Instance.CutsceneManager.PlayTimeline();
        //GetComponent<Outline>().enabled = false;

    }

    [SerializeField] int m_cutsceneIndex = 0;
    [SerializeField] UnityEvent m_action;

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(2f);
        Managers.Instance.TaskManager.NextTask();
        m_objectiveDone = true;
        m_action.Invoke();
        



    }




    //Must have for objectives
    [SerializeField] int m_forLoadingId;
    private void ForLoading()
    {
        Debug.Log("Forloading");
        Managers.Instance.AudioManager.PlayHere(m_workingSoundName, this.gameObject, true);
        m_action.Invoke();
    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
    }


}
