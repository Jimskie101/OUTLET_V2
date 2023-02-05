using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    GameManager m_gameManager;
    InputHandler m_inputHandler;
    bool m_controlsAreDown = false;
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
                m_gameManager.GameDirection = GameManager.Direction.front;
                m_vCamHolder.Priority = 10;
                m_frontCam.Priority = 20;
                m_vCamHolder = m_frontCam;
                break;
            case CameraPosition.left:
                m_gameManager.GameDirection = GameManager.Direction.left;
                m_vCamHolder.Priority = 10;
                m_leftCam.Priority = 20;
                m_vCamHolder = m_leftCam;
                break;
            case CameraPosition.back:
                m_gameManager.GameDirection = GameManager.Direction.back;
                m_vCamHolder.Priority = 10;
                m_backCam.Priority = 20;
                m_vCamHolder = m_backCam;
                break;
            case CameraPosition.right:
                m_gameManager.GameDirection = GameManager.Direction.right;
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
            if (!m_controlsAreDown)
            {
                m_controlsAreDown = true;
                Time.timeScale = 0f;
                m_inputHandler.enabled = false;
            }
        }
        else if (!m_camBrain.IsBlending)
        {
            if (m_controlsAreDown)
            {
                m_controlsAreDown = false;
                Time.timeScale = 1f;
                m_inputHandler.enabled = true;
                m_gameManager.ChangeGameDirection();
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
        m_gameManager = Managers.Instance.GameManager;
        m_inputHandler = Managers.Instance.InputHandler;
        //m_controlsAreDown = true;
        m_inputHandler.enabled = false;
        StartCoroutine(StartCamera());
        

    }
    IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(2f);
        ChangeCam();

    }
}
