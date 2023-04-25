using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Serialization;
public class Wires : MonoBehaviour
{

    //Reference Fields    
    [SerializeField] LineRenderer m_lineRenderer;
    [SerializeField] WireBase m_wireBase;

    //Serialized Fields
    [SerializeField] int m_quality;
    [SerializeField] float m_damper;
    [SerializeField] float m_strength;
    [SerializeField] float m_velocity;
    [SerializeField] float m_waveCount;
    [SerializeField] float m_waveHeight;
    [SerializeField] float m_duration;
    [SerializeField] AnimationCurve m_affectCurve;

    //Private
    private Spring m_spring;
    private Vector3 m_currentWirePosition;
    private Vector3 m_targetPoint;
    private Vector3 m_wireBasePosition;
    Vector3 m_direction;




    void Awake()
    {

        m_spring = new Spring();
        m_spring.SetTarget(0);
    }
    private void Start()
    {
        if (m_spring == null)
        {
            m_spring = new Spring();
            m_spring.SetTarget(0);
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

   

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!m_wireBase.Connect)
        {
            m_currentWirePosition = transform.position;
            m_spring.Reset();
            if (m_lineRenderer.positionCount > 0)
                m_lineRenderer.positionCount = 0;
            return;
        }

        if (m_lineRenderer.positionCount == 0)
        {
            m_spring.SetVelocity(m_velocity);
            m_lineRenderer.positionCount = m_quality + 1;
        }

        m_spring.SetDamper(m_damper);
        m_spring.SetStrength(m_strength);
        m_spring.Update(Time.deltaTime);

        
        m_targetPoint = m_wireBase.GetTargetPoint();
        m_wireBasePosition = transform.position;
        var up = Quaternion.LookRotation((m_targetPoint - m_wireBasePosition).normalized) * Vector3.up;

        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, m_direction, out hit))
        // {
        //     m_targetPoint = hit.point;
        //     m_direction = (transform.position - m_targetPoint).normalized;
        //     m_lineRenderer.SetPosition(1, m_targetPoint);
        // }

        m_currentWirePosition = Vector3.Lerp(m_currentWirePosition, m_targetPoint, Time.deltaTime * m_duration);

        for (var i = 0; i < m_quality + 1; i++)
        {
            var delta = i / (float)m_quality;
            var offset = up * m_waveHeight * Mathf.Sin(delta * m_waveCount * Mathf.PI) * m_spring.Value *
                         m_affectCurve.Evaluate(delta);

            m_lineRenderer.SetPosition(i, Vector3.Lerp(m_wireBasePosition, m_currentWirePosition, delta) + offset);
        }

    }
}
