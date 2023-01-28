using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EasyButtons;

public class PlayerMovement : MonoBehaviour
{
    //New Input System
    InputMaster m_inputMaster;

    //Reference Fields
    Rigidbody m_rb;
    InputHandler m_inputHandler;
    [SerializeField] CapsuleCollider m_CapsuleCollider;

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

    //Private Variables
    private float m_Vertical;
    private float m_Horizontal;
    private Vector3 m_MoveDirection;
    private float m_multiplier = 0f;


    //Serialized Fields
    [SerializeField] float m_AirMultiplier = 0.4f;
    [SerializeField] float m_GroundMultiplier = 0.4f;
    [SerializeField] float m_GroundDrag = 6f;
    [SerializeField] float m_AirDrag = 0f;

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
    bool m_brake = false;
    float m_friction= 0; // 1 means no friction, 0 stops instantly

    private void Awake()
    {
        m_inputHandler = Managers.Instance.InputHandler;
    }
    private void OnEnable()
    {
        m_inputHandler.PlayerMovementEnabled();
    }

    private void OnDisable()
    {
        m_inputHandler.PlayerMovementDisabled();
    }

    private void Start()
    {
        Initializer();

        ChangeGameDirection();

    }
    public void Initializer()
    {
        m_rb = GetComponent<Rigidbody>();
        m_groundChecker = GameObject.Find("GroundChecker").transform;
    }

    public Direction GameDirection;
    Vector3 m_xOrientation;
    Vector3 m_zOrientation;
    public enum Direction
    {
        front,
        left,
        back,
        right,
    }
    //Changes the game control direction
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
    //Set Move Direction Values
    public void GetDirection(Vector2 direction)
    {
        m_vertical = direction.y;
        m_horizontal = direction.x;
    }

    [SerializeField] Vector3 rbVel;

    private void Update()
    {
        rbVel = m_rb.velocity;
        GroundCheck();
        ControlDrag();
        MoveInput();

        RotatePlayer();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void KillMomentum()
    {
        if (m_brake)
        {
            m_velocity = m_rb.velocity;
            m_velocity.x *= m_friction; // modify only x and z components
            m_velocity.z *= m_friction;
            m_rb.velocity = m_velocity;
            // angularVelocity should be reduced as a whole:
            m_rb.angularVelocity *= m_friction;
        }
    }
    /////////// Custom Methods //////////

    //User Input like WASD, Joystick
    private void MoveInput()
    {

        m_inputDir = m_horizontal * m_xOrientation + m_vertical * m_zOrientation;


        m_speed = IsGrounded ? Speed : Speed * 0.75f;
        m_speed = m_isRunning ? m_speed * RunSpeedMultiplier : m_speed;

        // if (m_canJump && IsGrounded)
        // {
        //     m_canJump = false;
        //     Debug.Log("Jumped");
        //     m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravityPullValue);

        //     IsGrounded = false;
        // }
        if(m_inputDir == Vector3.zero) m_brake = true;
        else m_brake = false;
        if (!IsGrounded)
        {
            m_animator.SetBool("falling", true);
        }
        else m_animator.SetBool("falling", false);
        AnimationUpdate();

    }



    //Moving the player towards direction
    private void MovePlayer()
    {
        m_multiplier = IsGrounded ? m_GroundMultiplier : m_AirMultiplier;
        m_rb.AddForce(m_inputDir * m_speed * m_multiplier * Time.deltaTime,ForceMode.Acceleration);
        // m_rb.velocity = m_inputDir * m_speed * (m_multiplier * 0.1f);
    }


    //Checks if the player is airborne or on the ground
    private void GroundCheck()
    {
        IsGrounded = Physics.CheckSphere(new Vector3(transform.position.x,
            transform.position.y - (m_CapsuleCollider.height / 2), transform.position.z),
            m_groundCheckSphereRadius, m_groundLayer);
    }

    //Controls the air resistance when airborne or on the ground
    private void ControlDrag()
    {
        m_rb.drag = IsGrounded ? m_GroundDrag : m_AirDrag;
    }


    //Rotate the player
    Vector3 m_targetPos;
    public bool ConnectedToSource;
    public Transform TargetSource;
    public void RotatePlayer()
    {
        if (m_inputDir != Vector3.zero && !ConnectedToSource)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_inputDir), m_rotationTime);
        }
        else if (ConnectedToSource)
        {
            m_targetPos = TargetSource.position - transform.position;
            m_targetPos.y = 0;
            //Debug.DrawRay(transform.position, m_targetPos, Color.cyan);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_targetPos), m_rotationTime);
        }

    }

    //Jump Mechanic
    public void Jump()
    {
        if (IsGrounded)
            m_rb.AddForce(transform.up * m_jumpHeight, ForceMode.Impulse);
    }

    //Run
    public void Run() { m_isRunning = true; }
    public void Walk() { m_isRunning = false; }


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

    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_groundChecker.position, m_groundCheckSphereRadius);
    }
}
