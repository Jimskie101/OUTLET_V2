using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;
public class TaskManager : MonoBehaviour
{
    [SerializeField] string[] m_taskArray;
    [SerializeField] RectTransform m_textHolder;
    [SerializeField] TMPro.TextMeshProUGUI m_text;

    [SerializeField] private int m_taskNumber = -1;
    Sequence seq;

    private void OnEnable()
    {
        seq = DOTween.Sequence();
        m_text.text = m_taskArray[m_taskNumber];
        AnimateTask();
    }
    [Button]
    public void NextTask(bool waypointOnly = false)
    {
        if (!waypointOnly)
        {

            m_textHolder.DOLocalMoveY(300, 1.5f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                m_text.text = m_taskArray[m_taskNumber];

                AnimateTask();
            });

        }
        else
        {
            Managers.Instance.WaypointManager.NextWaypoint();
        }



    }


    public void AnimateTask()
    {

        m_textHolder.DOLocalMoveY(0, 1.5f).SetEase(Ease.InElastic).OnComplete(() =>
        Managers.Instance.WaypointManager.NextWaypoint());
    }
}
