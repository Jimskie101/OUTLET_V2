using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EasyButtons;

public class PlayerMovement : MonoBehaviour
{
    //New Input System
    InputMaster m_inputMaster;

    //Components
    CharacterController m_cc;

    //GameObjects and Transforms
    [SerializeField] Animator m_animator;
    [SerializeField] Transform m_groundChecker;

    //WASD / Joystick Analog Right
    float m_vertical;
    float m_horizontal;
    Vector3 m_inputDir;
    float m_speed;
    Vector3 m_rotateDir = Vector3.zero;
    [SerializeField] float m_rotationTime;

    //Jump
    [SerializeField] float m_jumpHeight;


    //Ground
    [SerializeField] float m_groundCheckSphereRadius;
    [SerializeField] LayerMask m_groundLayer;

    //Gravity
    [SerializeField] float m_gravityPullValue;
    Vector3 m_gravityVector;
    Vector3 m_velocity;

    //Public PlayerStats
    public float Speed;
    public float RunSpeedMultiplier;
    public bool IsGrounded = false;


    //Booleans
    bool m_canJump = false;
    bool m_isRunning = false;

    private void Awake()
    {
        m_inputMaster = new InputMaster();
        m_inputMaster.Player.Jump.performed += ctx => Jump();
        m_inputMaster.Player.Movement.performed += ctx => GetDirection(ctx.ReadValue<Vector2>());
        m_inputMaster.Player.Movement.canceled += ctx => GetDirection(Vector2.zero);
        m_inputMaster.Player.Sprint.performed += ctx => Run();
        m_inputMaster.Player.Sprint.canceled += ctx => Walk();
        m_inputMaster.Camera.NextCamera.canceled += ctx => Managers.Instance.CameraHandler.PrevCam();
        m_inputMaster.Camera.PreviousCamera.canceled += ctx => Managers.Instance.CameraHandler.NextCam();
        
       
    }
    


    private void OnEnable()
    {
        m_inputMaster.Player.Enable();
        m_inputMaster.Camera.Enable();
    }

    private void OnDisable()
    {
        m_inputMaster.Player.Disable();
        m_inputMaster.Camera.Disable();
    }

    private void Start()
    {
        Initializer();
        ChangeGameDirection();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();

    }



    [Button]
    public void Initializer()
    {
        m_cc = GetComponent<CharacterController>();
        m_groundChecker = GameObject.Find("GroundChecker").transform;
    }


    //Set Move Direction Values
    private void GetDirection(Vector2 direction)
    {
        m_vertical = direction.y;
        m_horizontal = direction.x;

    }

    public enum Direction
    {
        front,
        left,
        back,
        right,
    }

    public Direction GameDirection;
    Vector3 m_xOrientation;
    Vector3 m_zOrientation;


    //Changes the game control direction
    [Button]
    public void ChangeGameDirection()
    {
        switch (GameDirection)
        {
            case Direction.front:
                m_xOrientation = Vector3.right;
                m_zOrientation = Vector3.forward;
                break;

            case Direction.left:
                m_xOrientation = -Vector3.forward;
                m_zOrientation = Vector3.right;
                break;

            case Direction.back:
                m_xOrientation = -Vector3.right;
                m_zOrientation = -Vector3.forward;
                break;

            case Direction.right:
                m_xOrientation = Vector3.forward;
                m_zOrientation = -Vector3.right;
                break;
        }
    }


    //Moves the player
    private void MovePlayer()
    {
        GroundCheck();
        if (IsGrounded && m_velocity.y < 0)
        {
            m_velocity.y = -2f;
        }




        m_inputDir = m_horizontal * m_xOrientation + m_vertical * m_zOrientation;



        m_speed = IsGrounded ? Speed : Speed * 0.75f;
        m_speed = m_isRunning ? m_speed * RunSpeedMultiplier : m_speed;


        m_cc.Move(m_inputDir.normalized * m_speed * Time.deltaTime);


        if (m_canJump && IsGrounded)
        {
            m_canJump = false;
            Debug.Log("Jumped");
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravityPullValue);

            IsGrounded = false;
        }



        if (!IsGrounded)
        {   
            //Gravity
            if(m_velocity.y > m_gravityPullValue)
            m_velocity.y += m_gravityPullValue * Time.deltaTime;
            m_cc.Move(m_velocity * Time.deltaTime);
            m_animator.SetBool("falling", true);
        }
        else m_animator.SetBool("falling", false);



        AnimationUpdate();

    }

    private void RotatePlayer()
    {
        if (m_inputDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_inputDir), m_rotationTime);
            m_rotateDir.y = transform.localEulerAngles.y;
            transform.localEulerAngles = m_rotateDir;
        }
    }
    //Ground Checker
    private void GroundCheck()
    {
        IsGrounded = Physics.CheckSphere(m_groundChecker.position, m_groundCheckSphereRadius, m_groundLayer);
    }

    //Jump
    public void Jump()
    {
        if (IsGrounded)
        {
            m_canJump = true;
            m_animator.SetTrigger("jump");
        }

    }

    //Run
    public void Run() { m_isRunning = true; }
    public void Walk() { m_isRunning = false; }

    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_groundChecker.position, m_groundCheckSphereRadius);
    }

    private void AnimationUpdate()
    {
        if (m_inputDir != Vector3.zero && IsGrounded)
        {
            m_animator.SetBool("walking", true);
        }
        else if (m_inputDir == Vector3.zero || !IsGrounded)
        {
            m_animator.SetBool("walking", false);
        }

        if (m_inputDir != Vector3.zero && m_isRunning && IsGrounded)
        {
            m_animator.SetBool("running", true);
        }
        else if (!m_isRunning || !IsGrounded || m_inputDir == Vector3.zero)
        {
            m_animator.SetBool("running", false);
        }
    }


}
