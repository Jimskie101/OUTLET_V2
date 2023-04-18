using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 m_targetRotation;
    [SerializeField] float m_rotationDuration;
    [SerializeField] bool m_awakeActivate = false; 
    [SerializeField] int m_cutsceneIndex = 0; 

    WaitForSeconds waitingTime;
    bool m_isDone = false;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>

    private void Start()
    {
        waitingTime = new WaitForSeconds(2);
        Invoke("ObjectiveChecker", 0.1f);
        if(m_awakeActivate)
        {
            Activate();
        }
    }

    [Button]
    public void Activate()
    {
        if (!m_isDone)
        {
            m_isDone = true;
            //Update the GameManager objectives
            if (m_forLoadingId > Managers.Instance.GameManager.ObjectiveCounter)
            {
                Managers.Instance.TaskManager.NextTask();
                Managers.Instance.GameManager.ObjectiveCounter++;
                Managers.Instance.CutsceneManager.PlayCutscene(m_cutsceneIndex);
            }

            StartCoroutine(Rotating());
        }
    }


    IEnumerator Rotating()
    {
        yield return waitingTime;
        Managers.Instance.AudioManager.PlayHere(m_workingSoundName, this.gameObject);
        transform.DORotate(m_targetRotation, m_rotationDuration).SetUpdate(true);
    }

    [SerializeField] string m_workingSoundName;
    //Must have for objectives
    [SerializeField] int m_forLoadingId;
    private void ForLoading()
    {
        m_isDone = true;
        Debug.Log("Forloading",this.gameObject);
        transform.DORotate(m_targetRotation, 0f).SetUpdate(true);

    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
    }
}
