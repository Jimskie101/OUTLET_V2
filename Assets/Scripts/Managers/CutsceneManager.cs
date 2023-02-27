using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using EasyButtons;
public class CutsceneManager : MonoBehaviour
{
    CameraHandler m_cameraHandler;
    WaitForSeconds m_camResetTime;
    
    private void Start() {
        m_cameraHandler = Managers.Instance.CameraHandler;
        m_camResetTime = new WaitForSeconds(3f);
    }
    [SerializeField] Transform m_cameraHolder;
    [SerializeField] CinemachineVirtualCamera [] m_cutsceneCameras;
    CinemachineVirtualCamera m_tempCamera;

    [Button]
    private void GetCustceneCameras()
    {
        m_cutsceneCameras = m_cameraHolder.GetComponentsInChildren<CinemachineVirtualCamera>();
    }



    public void PlayCutscene(int cameraNum)
    {
        Managers.Instance.WaypointManager.HideMarker(true);
        m_cutsceneCameras[cameraNum].Priority = 40;
        m_tempCamera = m_cutsceneCameras[cameraNum];
        StartCoroutine(ResetCameraPriority());
        m_cameraHandler.CutsceneIsPlaying = true;
    }
    IEnumerator ResetCameraPriority()
    {
        yield return m_camResetTime;
        m_cameraHandler.CutsceneIsPlaying = false;
         m_tempCamera.Priority = 5;
         Managers.Instance.WaypointManager.HideMarker(false);
         
    }
}
