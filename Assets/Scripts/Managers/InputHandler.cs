using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Objects with Input Activation")]
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] HangingMovement m_hangingMovement;
    [SerializeField] CameraHandler m_cameraHandler;
    [SerializeField] WireBase m_hotWire;
    [SerializeField] WireBase m_neutralWire;



    //New Input System
    InputMaster m_inputMaster;

    private void Awake()
    {
        m_inputMaster = new InputMaster();
        m_inputMaster.Player.Jump.performed += ctx => m_playerMovement.Jump();
        m_inputMaster.Player.Movement.performed += ctx => m_playerMovement.GetDirection(ctx.ReadValue<Vector2>());
        m_inputMaster.Player.Movement.canceled += ctx => m_playerMovement.GetDirection(Vector2.zero);
        m_inputMaster.Player.Sprint.performed += ctx => m_playerMovement.Run();
        m_inputMaster.Player.Sprint.canceled += ctx => m_playerMovement.Walk();
        m_inputMaster.Camera.NextCamera.performed += ctx => m_cameraHandler.NextCam();
        m_inputMaster.Camera.PreviousCamera.performed += ctx => m_cameraHandler.PrevCam();
        m_inputMaster.Wires.Connect_Hot.performed += ctx => m_hotWire.StartGrapple();
        m_inputMaster.Wires.Connect_Neutral.performed += ctx => m_neutralWire.StartGrapple();
        m_inputMaster.Wires.Connect_Hot.canceled += ctx => m_hotWire.StopGrapple();
        m_inputMaster.Wires.Connect_Neutral.canceled += ctx => m_neutralWire.StopGrapple();
        m_inputMaster.UI.Continue.performed += ctx => Managers.Instance.UIManager.HideInfo();
        m_inputMaster.UI.Pause.performed += ctx => Managers.Instance.UIManager.PauseGame();
        //When Hanging Controls
        m_inputMaster.OnHook.Movement.performed += ctx => m_hangingMovement.GetDirection(ctx.ReadValue<Vector2>());
        m_inputMaster.OnHook.Movement.canceled += ctx => m_hangingMovement.GetDirection(Vector3.zero);
        m_inputMaster.OnHook.Jump.performed += ctx =>
        {
            m_hotWire.PullPlayer = true;
            m_neutralWire.PullPlayer = true;
        };
        m_inputMaster.OnHook.Jump.canceled += ctx =>
        {
            m_hotWire.PullPlayer = false;
            m_neutralWire.PullPlayer = false;
        };

    }



    private void OnEnable()
    {

        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
        m_inputMaster.Wires.Enable();
        m_inputMaster.UI.Enable();
        m_inputMaster.OnHook.Disable();
    }
    private void OnDisable()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
        m_inputMaster.UI.Disable();
        m_inputMaster.OnHook.Disable();
    }

    public void EndStage()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
        m_inputMaster.UI.Disable();
        m_inputMaster.OnHook.Disable();
    }


    //Ultra Disable All
    public void PlayerDead()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
        m_inputMaster.UI.Disable();
        m_inputMaster.OnHook.Disable();
    }


    //Disable All Game Inputs except Menu
    public void CameraRotating()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
        m_inputMaster.UI.Disable();
    }
    public void CameraStopped()
    {
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
        m_inputMaster.Wires.Enable();
        m_inputMaster.UI.Enable();

    }

    //Status of PlayerMovement Script
    public void UnShowingHint()
    {
        m_inputMaster.UI.Continue.Disable();
        m_inputMaster.UI.Pause.Enable();
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
    }

    public void ShowingHint()
    {
        m_inputMaster.UI.Continue.Enable();
        m_inputMaster.UI.Pause.Disable();
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
    }


    public void Pause()
    {
        m_inputMaster.UI.Pause.Enable();
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
        
    }

    public void UnPause()
    {
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
        m_inputMaster.Wires.Enable();
    }
    public void PlayerHanging()
    {
        m_inputMaster.OnHook.Enable();
        m_inputMaster.Player.Disable();
    }
    public void PlayerNotHanging()
    {
        m_inputMaster.OnHook.Disable();
        m_inputMaster.Player.Enable();
    }


}
