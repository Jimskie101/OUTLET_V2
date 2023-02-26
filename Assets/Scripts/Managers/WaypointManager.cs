using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class WaypointManager : MonoBehaviour
{

    [SerializeField] WaypointMarker m_waypointMarker;
    public int WaypointCounter = -1;

    [SerializeField] Transform[] m_taskWaypointsList;
    


    [Button]
    public void NextWaypoint()
    {
        if (WaypointCounter < m_taskWaypointsList.Length)
        {
            WaypointCounter++;
            m_waypointMarker.Target = m_taskWaypointsList[WaypointCounter];
            
        }
        

    }
}
