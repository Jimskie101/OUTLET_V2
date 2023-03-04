using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

[System.Serializable]
public class WaypointObject
{
    public Transform target;
    public bool isObject;
}




public class WaypointManager : MonoBehaviour
{

    [SerializeField] WaypointMarker m_waypointMarker;
    public int WaypointCounter = -1;

    [SerializeField] WaypointObject[] m_taskWaypointsList;

    private void Start()
    {
        for (int i = 0; i < m_taskWaypointsList.Length; i++)
        {
            if (!m_taskWaypointsList[i].isObject && i != WaypointCounter+1)
            {
                m_taskWaypointsList[i].target.gameObject.SetActive(false);
            }
        }
    }

    [Button]
    public void NextWaypoint()
    {
        
        if (WaypointCounter < m_taskWaypointsList.Length - 1)
        {
            WaypointCounter++;
            if (!m_taskWaypointsList[WaypointCounter].isObject)
            {
                m_taskWaypointsList[WaypointCounter].target.gameObject.SetActive(true);
            }

            m_waypointMarker.Target = m_taskWaypointsList[WaypointCounter].target;


        }


    }

    [SerializeField] Transform m_tempTarget;
    public void HideMarker(bool hidden)
    {
        if (hidden)
        {
            m_tempTarget = m_waypointMarker.Target;
            m_waypointMarker.Target = null;
        }
        else
        {
            m_waypointMarker.Target = m_tempTarget;
            m_tempTarget = null;
        }


    }
}
