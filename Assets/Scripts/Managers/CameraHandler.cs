using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Cinemachine;

public enum CameraPosition
{
    front,
    left,
    back,
    right,
}

public class CameraHandler : MonoBehaviour
{
    GameManager m_gameManager;
    InputHandler m_inputHandler;
    bool m_controlsAreDown = false;
    public Camera MainCamera;
    [SerializeField] CinemachineBrain m_camBrain;
    [SerializeField] CinemachineVirtualCamera m_frontCam;
    [SerializeField] CinemachineVirtualCamera m_leftCam;
    [SerializeField] CinemachineVirtualCamera m_backCam;
    [SerializeField] CinemachineVirtualCamera m_rightCam;

    [HideInInspector] public bool CutsceneIsPlaying = false;


    public CameraPosition CamPosition;
    [SerializeField] CinemachineVirtualCamera m_vCamHolder;

    [Button]
    public void ChangeCam()
    {

        switch (CamPosition)
        {
            case CameraPosition.front:
                if (m_gameManager == null)
                    Managers.Instance.GameManager.GameDirection = Direction.front;
                else m_gameManager.GameDirection = Direction.front;
                m_vCamHolder.Priority = 10;
                m_frontCam.Priority = 20;
                m_vCamHolder = m_frontCam;
                break;
            case CameraPosition.left:
                if (m_gameManager == null)
                    Managers.Instance.GameManager.GameDirection = Direction.left;
                else m_gameManager.GameDirection = Direction.left;
                m_vCamHolder.Priority = 10;
                m_leftCam.Priority = 20;
                m_vCamHolder = m_leftCam;
                break;
            case CameraPosition.back:
                if (m_gameManager == null)
                    Managers.Instance.GameManager.GameDirection = Direction.back;
                else m_gameManager.GameDirection = Direction.back;
                m_vCamHolder.Priority = 10;
                m_backCam.Priority = 20;
                m_vCamHolder = m_backCam;
                break;
            case CameraPosition.right:
                if (m_gameManager == null)
                    Managers.Instance.GameManager.GameDirection = Direction.right;
                else m_gameManager.GameDirection = Direction.right;
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
                //m_inputHandler.CameraRotating();
            }
        }
        else if (!m_camBrain.IsBlending)
        {
            if (m_controlsAreDown)
            {
                m_controlsAreDown = false;
                Time.timeScale = 1f;
                if (!CutsceneIsPlaying)
                    m_inputHandler.CameraStopped();
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
        if (m_gameManager != null)
            if (!m_gameManager.NoCutscene)
            {
                m_inputHandler.CameraRotating();
                StartCoroutine(StartCamera());
            }


    }
    IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(2f);
        ChangeCam();

    }
}
