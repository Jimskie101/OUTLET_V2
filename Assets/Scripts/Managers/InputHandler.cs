using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Objects with Input Activation")]
    [SerializeField] PlayerMovement m_playerMovement;
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
    }



    private void OnEnable()
    {
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
        m_inputMaster.Wires.Enable();
    }
    private void OnDisable()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
        m_inputMaster.Wires.Disable();
    }


    //Status of PlayerMovement Script
    public void PlayerMovementEnabled()
    {
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
    }

    public void PlayerMovementDisabled()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
    }

}
