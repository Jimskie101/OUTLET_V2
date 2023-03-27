using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class MovePlatform : MonoBehaviour
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


    [SerializeField] bool m_repeatedCalls = false;
    [SerializeField] int m_callsCount = 0;
    [SerializeField] int m_calls = 0;

    [SerializeField] bool m_triggerCutscene = false;
    [SerializeField] int m_cutsceneIndex = 0;
    
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
    public void MoveMe(bool update = false)
    {
        if (m_repeatedCalls)
        {
            if (m_calls >= m_callsCount)
            {
                if(m_triggerCutscene)
                Managers.Instance.CutsceneManager.PlayCutscene(m_cutsceneIndex);
                m_transform.DOLocalMove(m_targetPosition, m_moveDuration).SetUpdate(update);
            }
            else
            {
                m_calls++;
            }

        }
        else m_transform.DOLocalMove(m_targetPosition, m_moveDuration).SetUpdate(update);

    }
    public void PutMeBack()
    {
        m_transform.DOLocalMove(m_defaultPosition, m_moveDuration);
    }

    public void RotateToTarget()
    {
        if (m_repeatedCalls)
        {
            if (m_calls >= m_callsCount)
            {
                m_transform.DOLocalRotate(m_targetRotation, m_rotateDuration);
            }
             else
            {
                m_calls++;
            }

        }
        else m_transform.DOLocalRotate(m_targetRotation, m_rotateDuration);

    }
    public void RotateToOrigin()
    {
        transform.DOLocalRotate(m_defaultRotation, m_rotateDuration);
    }


}
