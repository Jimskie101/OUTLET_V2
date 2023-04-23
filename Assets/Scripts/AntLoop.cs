using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class AntLoop : MonoBehaviour
{
    [SerializeField] Vector3 m_startingPoint;
    [SerializeField] Vector3 m_targetPoint;
    [SerializeField] float m_duration;
    [SerializeField] Ease m_ease;
    Vector3 m_defaultPos;
    Tween m_loopAnimation;

    [SerializeField] bool m_playOnAwake = false;
    private void Start()
    {
        m_defaultPos = transform.position;
        if (m_playOnAwake)
        {
            LoopAnimation();
        }
    }
    [Button]
    public void LoopAnimation()
    {
        m_loopAnimation = transform.DOLocalMove( m_targetPoint, m_duration).SetLoops(-1).SetEase(m_ease);
    }
    [Button]
    public void StopLoop()
    {
        m_loopAnimation.Kill();
        transform.position = m_defaultPos;
    }

}
