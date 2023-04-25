using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using DG.Tweening;

public class TrainAnimation : MonoBehaviour
{
    [SerializeField] Transform []  m_wheels;
    [SerializeField] Transform m_bodyParent;
    [SerializeField] Transform m_bodyChild;
    [SerializeField] Vector3 m_wheelRotation;
    [SerializeField] float m_bodyTargetPosition;
    [SerializeField] float m_wheelSpeed;
    [SerializeField] float m_bodySpeed;
    [SerializeField] Transform m_player;
    [SerializeField] GameObject [] m_Lights;

    WaitForSeconds m_powerTime;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_powerTime = new WaitForSeconds(4);
    }

    [Button]
    public void MoveTrain()
    {
        //Managers.Instance.CutsceneManager.PlayTimeline();
        StartCoroutine(Action());
        
    }

    IEnumerator Action()
    {
        yield return m_powerTime;
        
        Managers.Instance.AudioManager.PlayHere("train", m_bodyChild.gameObject);
        foreach (GameObject l in m_Lights)
        {
            l.gameObject.SetActive(true);
        }
        foreach(Transform w in m_wheels)
        {
            w.DORotate(m_wheelRotation, m_wheelSpeed,RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }

        m_bodyChild.SetParent(m_bodyParent);
        if(m_player != null) m_player.SetParent(m_bodyParent);
        m_bodyParent.DOMoveZ(m_bodyTargetPosition,m_bodySpeed).SetEase(Ease.Linear);
        Managers.Instance.UIManager.FadeToBlack();
    }

    
}
