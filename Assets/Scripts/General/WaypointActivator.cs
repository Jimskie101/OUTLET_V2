using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointActivator : MonoBehaviour
{
    TaskManager m_taskManager;

    [SerializeField] bool m_waypointOnly;
    

    private void Start()
    {
        m_taskManager = Managers.Instance.TaskManager;
        if(int.Parse(this.name) < Managers.Instance.WaypointManager.WaypointCounter)
        {
            this.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_taskManager.NextTask(m_waypointOnly);
            //Debug.Log("Task Finished");
            this.gameObject.SetActive(false);
        }
    }
}
