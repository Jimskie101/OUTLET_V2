using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingMovement : MonoBehaviour
{
    GameManager m_gameManager;
    public float m_speed = 10f;
    private Rigidbody m_rb;
    private Vector2 m_movementInput;
    Vector3 m_movement;
    float m_horizontal;
    float m_vertical;
    [SerializeField] Transform m_player;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Managers.Instance.InputHandler.PlayerHanging();
    }

    private void OnDisable()
    {
        Managers.Instance.InputHandler.PlayerNotHanging();
    }

    private void Start()
    {
        m_gameManager = Managers.Instance.GameManager;
    }

    public void GetDirection(Vector2 direction)
    {
        m_horizontal = direction.x;
        m_vertical = direction.y;
    }
    private void Update()
    {
        m_player.rotation = Quaternion.identity;
        m_movement = m_horizontal * m_gameManager.XOrientation + m_vertical * m_gameManager.ZOrientation;
    }

    private void FixedUpdate()
    {
        m_rb.velocity += m_movement * m_speed * 0.01f;
    }
}
