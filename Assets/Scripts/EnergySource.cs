using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class EnergySource : MonoBehaviour
{
    public float m_initialCharge;
    [SerializeField] SphereCollider m_hot;
    [SerializeField] SphereCollider m_neutral;

    public Outline Outline;

    public float Charge;

    private bool isTaskDone = false;
    [SerializeField] bool isATask = false;
    private void OnEnable()
    {
        Charge = m_initialCharge;
    }

    [Button]
    private void InitializeFields()
    {
        m_hot = transform.Find("SocketHot").GetComponent<SphereCollider>();
        m_neutral = transform.Find("SocketNeutral").GetComponent<SphereCollider>();
        Outline = GetComponent<Outline>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (Charge < 1 && m_hot.enabled)
        {
            m_hot.enabled = false;
            m_neutral.enabled = false;
            //m_outline.enabled = false;
            Outline.OutlineWidth = 0;
            if(!isTaskDone && isATask)
            {
                isTaskDone = true; 
                //Managers.Instance.TaskManager.NextTask();
                this.enabled = false;
            }
        }
    }
}
