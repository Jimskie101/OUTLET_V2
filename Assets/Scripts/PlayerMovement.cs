using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EasyButtons;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerData m_playerData;
    //New Input System
    InputMaster m_inputMaster;

    //Components
    CharacterController m_cc;
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





    Vector3 m_gravityVector;
    Vector3 m_velocity;

    public bool IsGrounded = false;


    //Booleans
    bool m_canJump = false;
    bool m_isRunning = false;


    private void Awake()
    {

    }

    private void OnEnable()
    {
    }




    private void Start()
    {
        Initializer();

        m_gameManager.ChangeGameDirection();

    }

    [Header("Fall Damage")]

    [SerializeField] float startYPos = 0;
    [SerializeField] float endYPos = 0;
    bool firstCall = true;
    bool damaged = false;
    public bool FallingDisabled;

    private void FallCheck()
    {
        if (!IsGrounded)
        {
            if (transform.position.y > startYPos)
            {
                firstCall = true;
            }

            if (firstCall)
            {
                startYPos = transform.position.y;
                firstCall = false;
                damaged = true;
            }
        }

        if (IsGrounded)
        {
            endYPos = transform.position.y;
            if (startYPos - endYPos > m_playerData.FallThresholdLimit)
            {
                if (damaged)
                {
                    Debug.Log("Do Damage: " + (startYPos - endYPos));
                    m_playerScript.IsDead = true;
                    damaged = false;
                    firstCall = true;
                }
            }
        }
    }



    private void Update()
    {
        if (!FallingDisabled)
        {
            FallCheck();
        }
        if (IsGrounded)
        {
            if (startYPos != 0) startYPos = 0;
            if (endYPos != 0) endYPos = 0;
        }
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



        m_speed = IsGrounded ? m_playerData.Speed : m_playerData.Speed * 0.75f;
        m_speed = m_isRunning ? m_speed * m_playerData.RunSpeedMultiplier : m_speed;

        m_cc.Move(m_inputDir.normalized * m_speed * Time.deltaTime);


        if (m_canJump && IsGrounded)
        {
            m_canJump = false;

            Debug.Log("Jumped");
            m_velocity.y = Mathf.Sqrt(m_playerData.JumpHeight * -2f * m_playerData.GravityPullValue);

            IsGrounded = false;
        }



        if (!IsGrounded || m_cc.velocity.y < 0)
        {
            //Gravity
            if (m_velocity.y > m_playerData.GravityPullValue)
                m_velocity.y += m_playerData.GravityPullValue * Time.deltaTime;
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
        if (ConnectedToSource)
        {
            m_targetPos = TargetSource.position - transform.position;
            m_targetPos.y = 0;
            //Debug.DrawRay(transform.position, m_targetPos, Color.cyan);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_targetPos), m_playerData.RotationTime);
        }

        else if (m_inputDir != Vector3.zero && !ConnectedToSource)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_inputDir), m_playerData.RotationTime);
            RotateDust();
        }



    }
    bool m_landSoundPlayed = false;
    //Ground Checker
    private void GroundCheck()
    {
        IsGrounded = Physics.CheckSphere(m_groundChecker.position, m_playerData.GroundCheckSphereRadius, m_playerData.GroundLayer);
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
        Gizmos.DrawWireSphere(m_groundChecker.position, m_playerData.GroundCheckSphereRadius);
    }


    bool m_deadAlready = false;
    [HideInInspector]

    private void AnimationUpdate()
    {

        if (m_inputDir != Vector3.zero && IsGrounded)
        {
            m_playerScript.ChangeStateMultiplier = 3f;
            m_animator.SetBool("walking", true);
            m_dustMain.startLifetime = 2f;
            m_dustMain.simulationSpeed = 0.5f;



        }
        else if (m_inputDir == Vector3.zero || !IsGrounded)
        {
            m_playerScript.ChangeStateMultiplier = 1f;
            m_animator.SetBool("walking", false);


        }

        if (m_inputDir != Vector3.zero && m_isRunning && IsGrounded)
        {
            m_playerScript.ChangeStateMultiplier = 5f;
            m_animator.SetBool("running", true);
            m_dustMain.startLifetime = 1f;
            m_dustMain.simulationSpeed = 1f;
        }
        else if (!m_isRunning || !IsGrounded || m_inputDir == Vector3.zero)
        {
            m_animator.SetBool("running", false);

        }
        if (m_playerScript.IsDead && !m_deadAlready)
        {
            m_deadAlready = true;
            m_playerScript.LifePercentage -= 100;
            m_playerScript.ConsumeHealth();
            m_animator.SetTrigger("dead");
            Managers.Instance.InputHandler.PlayerDead();
            Managers.Instance.UIManager.FadeToBlack();
            StartCoroutine(Managers.Instance.UIManager.DeathScreen());
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
