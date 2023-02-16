using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class MovePlatform : MonoBehaviour
{
    [Header("Mover")]
    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_moveDuration = 0;

    [Header("Rotator")]
    [SerializeField] Vector3 m_targetRotation;
    [SerializeField] Vector3 m_defaultRotation;
    [SerializeField] float m_rotateDuration = 0;


    [Button]
    public void MoveMe()
    {
        transform.DOLocalMove(m_targetPosition, m_moveDuration);
    }
    public void PutMeBack()
    {
        transform.DOLocalMove(m_defaultPosition, m_moveDuration);
    }

    public void RotateToTarget()
    {
        transform.DOLocalRotate(m_targetRotation, m_rotateDuration);
    }
    public void RotateToOrigin()
    {
        transform.DOLocalRotate(m_defaultRotation, m_rotateDuration);
    }


}
