using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class WaypointManager : MonoBehaviour
{

    [SerializeField] WaypointMarker m_waypointMarker;
    private int m_waypointCounter = -1;

    [SerializeField] Transform[] m_taskWaypointsList;
    


    [Button]
    public void NextWaypoint()
    {
        if (m_waypointCounter < m_taskWaypointsList.Length)
        {
            m_waypointCounter++;
            m_waypointMarker.Target = m_taskWaypointsList[m_waypointCounter];
            
        }
        

    }
}
