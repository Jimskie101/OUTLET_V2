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

    public int TaskNumber = 0;

    private void Start()
    {
        m_text.text = m_taskArray[TaskNumber];
        Managers.Instance.WaypointManager.NextWaypoint();
        AnimateTask();
        
    }
    [Button]
    public void NextTask(bool waypointOnly = false)
    {
        Managers.Instance.WaypointManager.NextWaypoint();
        if (!waypointOnly)
        {
            TaskNumber++;
            m_textHolder.DOLocalMoveY(300, 1f).OnComplete(() =>
            {
                m_text.text = m_taskArray[TaskNumber];

                AnimateTask();
            });

        }


    }


    public void AnimateTask()
    {
        
        m_textHolder.DOLocalMoveY(0, 1.5f).SetEase(Ease.InElastic);
    }
}
