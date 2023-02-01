using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{

    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] CinemachineBrain m_camBrain;
    [SerializeField] CinemachineVirtualCamera m_frontCam;
    [SerializeField] CinemachineVirtualCamera m_leftCam;
    [SerializeField] CinemachineVirtualCamera m_backCam;
    [SerializeField] CinemachineVirtualCamera m_rightCam;

    public enum CameraPosition
    {
        front,
        left,
        back,
        right,
    }
    public CameraPosition CamPosition;
    [SerializeField] CinemachineVirtualCamera m_vCamHolder;

    [Button]
    private void ChangeCam()
    {

        switch (CamPosition)
        {
            case CameraPosition.front:
                m_playerMovement.GameDirection = PlayerMovement.Direction.front;
                m_vCamHolder.Priority = 10;
                m_frontCam.Priority = 20;
                m_vCamHolder = m_frontCam;
                break;
            case CameraPosition.left:
                m_playerMovement.GameDirection = PlayerMovement.Direction.left;
                m_vCamHolder.Priority = 10;
                m_leftCam.Priority = 20;
                m_vCamHolder = m_leftCam;
                break;
            case CameraPosition.back:
                m_playerMovement.GameDirection = PlayerMovement.Direction.back;
                m_vCamHolder.Priority = 10;
                m_backCam.Priority = 20;
                m_vCamHolder = m_backCam;
                break;
            case CameraPosition.right:
                m_playerMovement.GameDirection = PlayerMovement.Direction.right;
                m_vCamHolder.Priority = 10;
                m_rightCam.Priority = 20;
                m_vCamHolder = m_rightCam;
                break;
        }

    }

    private void LateUpdate()
    {
        if (m_camBrain.IsBlending)
        {
            if (m_playerMovement.enabled == true)
            {
                m_playerMovement.enabled = false;
                Time.timeScale = 0f;
            }
        }
        if (!m_camBrain.IsBlending)
        {
            if (m_playerMovement.enabled == false)
            {
                m_playerMovement.enabled = true;
                Time.timeScale = 1f;
                m_playerMovement.ChangeGameDirection();
            }

        }
    }




    public void NextCam()
    {
        if (CamPosition != CameraPosition.right)
        {
            CamPosition++;
        }
        else CamPosition = CameraPosition.front;

        ChangeCam();
    }
    public void PrevCam()
    {
        if (CamPosition != CameraPosition.front)
        {
            CamPosition--;
            ChangeCam();
        }
        else CamPosition = CameraPosition.right;
        ChangeCam();
    }


    private void Start()
    {
        StartCoroutine(StartCamera());
        

    }
    IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(2f);
        ChangeCam();

    }
}
