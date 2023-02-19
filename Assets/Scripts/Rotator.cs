using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 m_targetRotation;
    [SerializeField] float m_rotationDuration;

    WaitForSeconds waitingTime;
    bool m_isDone = false;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>

    private void Start()
    {
        waitingTime = new WaitForSeconds(2);
    }
    [Button]
    public void Activate()
    {
        if (!m_isDone)
        {
            m_isDone = true;
            Managers.Instance.CutsceneManager.PlayCutscene(2);
            StartCoroutine(Rotating());
        }
    }


    IEnumerator Rotating()
    {
        yield return waitingTime;
        Managers.Instance.AudioManager.PlayHere("fence_door", this.gameObject, true);
        transform.DORotate(m_targetRotation, m_rotationDuration).SetUpdate(true);
    }
}
