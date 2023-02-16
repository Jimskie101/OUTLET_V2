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
        NextTask();
    }
    [Button]
    public void NextTask(bool b_waypointOnly = false)
    {
        if (!b_waypointOnly)
        {
            m_taskNumber++;
            SlideTween(m_textHolder, 179, -115.5f, 0.2f, 179, 142.9f, 0.5f).OnComplete(() =>
            {
                m_text.text = m_taskArray[m_taskNumber];
                AnimateTask();
            });

        }


        Managers.Instance.WaypointManager.NextWaypoint();
    }

    public Sequence SlideTween(RectTransform ui, float x, float y, float duration1, float addX, float addY, float duration2)
    {
        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(ui.DOAnchorPos(new Vector2(x, y), duration1));
        seq.Append(ui.DOAnchorPos(new Vector2(addX, addY), duration2));

        return seq;
    }
    public void AnimateTask()
    {

        SlideTween(m_textHolder, 179, -115.5f, 0.5f, 179, -65.5f, 0.2f);
    }
}
