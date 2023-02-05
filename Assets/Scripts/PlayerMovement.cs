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
    InputHandler m_inputHandler;
    PlayerScript m_playerScript;
    GameManager m_gameManager;

    AudioManager m_audioManager;

    //GameObjects and Transforms
    [SerializeField] Animator m_animator;
    [SerializeField] Transform m_groundChecker;
    public GameObject FakeRigidBody;

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

        m_gameManager.ChangeGameDirection();

    }

    private void Update()
    {
        if (m_cc.enabled)
            MovePlayer();
        RotatePlayer();

    }



    [Button]
    public void Initializer()
    {
        m_cc = GetComponent<CharacterController>();
        m_playerScript = GetComponent<PlayerScript>();
        m_gameManager = Managers.Instance.GameManager;
        m_groundChecker = GameObject.Find("GroundChecker").transform;
        m_audioManager = Managers.Instance.AudioManager;
        m_dustShape = m_dust.shape;
        m_dustEmission = m_dust.emission;
        m_dustMain = m_dust.main;
    }


    //Set Move Direction Values
    public void GetDirection(Vector2 direction)
    {
        m_vertical = direction.y;
        m_horizontal = direction.x;

    }









    //Moves the player
    private void MovePlayer()
    {
        GroundCheck();




        m_inputDir = m_horizontal * m_gameManager.XOrientation + m_vertical * m_gameManager.ZOrientation;



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



        if (!IsGrounded || m_cc.velocity.y < 0)
        {
            //Gravity
            if (m_velocity.y > m_gravityPullValue)
                m_velocity.y += m_gravityPullValue * Time.deltaTime;
            m_cc.Move(m_velocity * Time.deltaTime);
            m_animator.SetBool("falling", true);
        }
        else m_animator.SetBool("falling", false);
        m_cc.Move(m_velocity * Time.deltaTime);

        AnimationUpdate();
        DustParticle();

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
            RotateDust();
        }
        else if (ConnectedToSource)
        {
            m_targetPos = TargetSource.position - transform.position;
            m_targetPos.y = 0;
            //Debug.DrawRay(transform.position, m_targetPos, Color.cyan);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_targetPos), m_rotationTime);
        }



    }
    bool m_landSoundPlayed = false;
    //Ground Checker
    private void GroundCheck()
    {
        IsGrounded = Physics.CheckSphere(m_groundChecker.position, m_groundCheckSphereRadius, m_groundLayer);
        m_animator.SetBool("grounded", IsGrounded);
        if (IsGrounded && !m_landSoundPlayed)
        {
            m_landSoundPlayed = true;
            m_audioManager.PlayHere("rl_walk", this.gameObject);
        }
        else if (!IsGrounded && m_landSoundPlayed)
        {
            m_landSoundPlayed = false;
        }
    }

    //Jump
    public void Jump()
    {
        if (IsGrounded)
        {
            m_jumpFX.gameObject.transform.position = transform.position;
            m_jumpFX.Play();
            
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


    bool m_deadAlready = false;
    private void AnimationUpdate()
    {
        if (m_inputDir != Vector3.zero && IsGrounded)
        {
            m_playerScript.PlayerStateDimMultiplier = 3f;
            m_animator.SetBool("walking", true);
            m_dustMain.startLifetime = 2f;
            m_dustMain.simulationSpeed = 0.5f;



        }
        else if (m_inputDir == Vector3.zero || !IsGrounded)
        {
            m_playerScript.PlayerStateDimMultiplier = 1f;
            m_animator.SetBool("walking", false);


        }

        if (m_inputDir != Vector3.zero && m_isRunning && IsGrounded)
        {
            m_playerScript.PlayerStateDimMultiplier = 5f;
            m_animator.SetBool("running", true);
            m_dustMain.startLifetime = 1f;
            m_dustMain.simulationSpeed = 1f;
        }
        else if (!m_isRunning || !IsGrounded || m_inputDir == Vector3.zero)
        {
            m_animator.SetBool("running", false);

        }
        if (m_playerScript.isDead && !m_deadAlready)
        {
            m_deadAlready = true;
            m_animator.SetTrigger("dead");
            Managers.Instance.InputHandler.DisableAllInGameInput();
        }
    }

    //Particle system that spawns foot dust
    [Header("Particles")]
    [SerializeField] ParticleSystem m_dust;
    ParticleSystem.ShapeModule m_dustShape;
    ParticleSystem.EmissionModule m_dustEmission;
    ParticleSystem.MainModule m_dustMain;


    private void RotateDust()
    {
        m_dustShape.rotation = transform.rotation.eulerAngles;
    }
    Vector3 m_ccMove;
    private void DustParticle()
    {
        m_ccMove.x = m_inputDir.x;
        m_ccMove.z = m_inputDir.z;

        if (m_ccMove.magnitude > 0 && IsGrounded)
        {
            m_dustEmission.rateOverTime = 10;
        }
        else if (m_ccMove.magnitude <= 0 || !IsGrounded)
        {
            m_dustEmission.rateOverTime = 0;
        }
    }
    

    [SerializeField] ParticleSystem m_jumpFX;

}
