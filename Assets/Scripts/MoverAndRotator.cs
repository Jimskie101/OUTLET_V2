using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class MoverAndRotator : MonoBehaviour
{
    [SerializeField] bool m_runOnEnable;
    [SerializeField] bool m_move;
    [SerializeField] bool m_rotate;
    [SerializeField] bool m_parent = false;
    Transform m_transform;
    [Header("Mover")]
    [SerializeField] Vector3 m_targetPosition;
    [SerializeField] Vector3 m_defaultPosition;
    [SerializeField] float m_moveDuration = 0;

    [Header("Rotator")]
    [SerializeField] Vector3 m_targetRotation;
    [SerializeField] Vector3 m_defaultRotation;
    [SerializeField] float m_rotateDuration = 0;


    
    private void OnEnable()
    {
        m_transform = m_parent ? transform.parent : transform;

        if (m_runOnEnable)
        {
            if (m_move)
            {
                MoveMe();
            }
            if (m_rotate)
            {
                RotateToTarget();
            }
        }

    }

    [Button]
    public void MoveMe(bool forloading = false)
    {
        if(forloading) m_transform.DOLocalMove(m_targetPosition, 0).SetUpdate(true);
        else m_transform.DOLocalMove(m_targetPosition, m_moveDuration).SetUpdate(true);

    }
    public void PutMeBack()
    {
        m_transform.DOLocalMove(m_defaultPosition, m_moveDuration);
    }

    public void RotateToTarget(bool forloading = false)
    {
       if(forloading) m_transform.DOLocalRotate(m_targetRotation, 0);
       else m_transform.DOLocalRotate(m_targetRotation, m_rotateDuration);

    }
    public void RotateToOrigin()
    {
        transform.DOLocalRotate(m_defaultRotation, m_rotateDuration);
    }

    

}
