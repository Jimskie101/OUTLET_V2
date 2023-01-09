using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PlayerMovement : MonoBehaviour
{
    //Components
    CharacterController m_cc;

    //GameObjects and Transforms
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
    public bool IsGrounded = false;


    //Booleans

    private void Start()
    {
        Initializer();
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


    //Moves the player
    private void MovePlayer()
    {
        GroundCheck();
        if (IsGrounded && m_velocity.y < 0)
        {
            m_velocity.y = -2f;
        }



        m_vertical = Input.GetAxisRaw("Vertical");
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_inputDir = m_horizontal * Vector3.right + m_vertical * Vector3.forward;
        m_speed = IsGrounded ? Speed : Speed * 0.75f;
        m_cc.Move(m_inputDir.normalized * m_speed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            Debug.Log("Jumped");
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravityPullValue);
            IsGrounded = false;
        }


        

        if (!IsGrounded)
        {
            //Gravity
            m_velocity.y += m_gravityPullValue * Time.deltaTime;
            m_cc.Move(m_velocity * Time.deltaTime);
        }





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

    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_groundChecker.position, m_groundCheckSphereRadius);
    }




}
