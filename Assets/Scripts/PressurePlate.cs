using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

enum direction
{
    x, y, z
}


public class PressurePlate : MonoBehaviour
{
    [SerializeField] float m_PressLimit;
    [SerializeField] float m_PressDuration;
    [SerializeField] float m_ResetDuration;
    [SerializeField] float m_StateResetDelay = 0.01f;
    [SerializeField] direction m_dir;
    [SerializeField] LayerMask m_layerMask;

    [SerializeField] UnityEvent m_DoThisIfPressed;
    [SerializeField] UnityEvent m_DoThisIfUnpressed;

    float m_timer;

    [SerializeField] bool m_isPressed = false;
    bool m_shouldReset = false;

    float m_defaultPos;


    private void ResetTimer()
    {
        if (m_timer > 0)
            m_timer -= Time.deltaTime;
        else if (m_timer <= 0)
        {
            Reseting(m_dir);
        }

    }

    private void OnEnable()
    {

        m_timer = m_StateResetDelay;
        switch (m_dir)
        {
            case direction.x: m_defaultPos = transform.parent.localPosition.x; break;
            case direction.y: m_defaultPos = transform.parent.localPosition.y; break;
            case direction.z: m_defaultPos = transform.parent.localPosition.z; break;
        }
    }

    private void Update()
    {
        if (m_colliderCount == 0 && m_isPressed == true)
        {
            Reseting(m_dir);
        }
        if (m_shouldReset)
        {
            ResetTimer();
        }
        else if (m_timer != m_StateResetDelay)
            m_timer = m_StateResetDelay;
    }

    [SerializeField] int m_colliderCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (m_layerMask == (m_layerMask | (1 << other.gameObject.layer)))
        {
            m_colliderCount++;
            if (!m_isPressed)
            {
                m_shouldReset = false;
                Lowering(m_dir);
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (m_layerMask == (m_layerMask | (1 << other.gameObject.layer)))
        {
            m_colliderCount--;
            if (m_colliderCount == 0) m_shouldReset = true;

        }
    }

    private void Lowering(direction dir)
    {
        switch (dir)
        {
            case direction.x: transform.parent.DOLocalMoveX(m_defaultPos - m_PressLimit, m_PressDuration).OnComplete(() => Pressed()); break;
            case direction.y: transform.parent.DOLocalMoveY(m_defaultPos - m_PressLimit, m_PressDuration).OnComplete(() => Pressed()); break;
            case direction.z: transform.parent.DOLocalMoveZ(m_defaultPos - m_PressLimit, m_PressDuration).OnComplete(() => Pressed()); break;
        }
    }
    private void Reseting(direction dir)
    {
        switch (dir)
        {
            case direction.x: transform.parent.DOLocalMoveX(m_defaultPos, m_ResetDuration).OnComplete(() => { m_isPressed = false; m_shouldReset = false; m_DoThisIfUnpressed.Invoke(); }); break;
            case direction.y: transform.parent.DOLocalMoveY(m_defaultPos, m_ResetDuration).OnComplete(() => { m_isPressed = false; m_shouldReset = false; m_DoThisIfUnpressed.Invoke(); }); break;
            case direction.z: transform.parent.DOLocalMoveZ(m_defaultPos, m_ResetDuration).OnComplete(() => { m_isPressed = false; m_shouldReset = false; m_DoThisIfUnpressed.Invoke(); }); break;
        }

    }


    private void Pressed()
    {
        m_isPressed = true;
        m_shouldReset = false;
        m_DoThisIfPressed.Invoke();
    }


}
