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
        
    }


    public void Refill()
    {
        Charge = 1000000;
        Outline.OutlineWidth = 2;
        
    }

}
