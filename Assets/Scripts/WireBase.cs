using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireBase : MonoBehaviour
{
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] Vector3 m_targetPoint;
    [SerializeField] LayerMask m_interactableLayerMask;
    [SerializeField] float m_socketDetectionRange;
    [SerializeField] float m_wireLength;
    public Transform TargetObject = null;
    public Transform Source = null;
    Vector3 m_lookPosition;
    public bool Connect = false;
    public bool IsHotWire = false;
    Collider[] m_hitColliders;
    private SpringJoint joint;
    private string wireTag = "";
    private WaitForSeconds connectTime;
    [SerializeField] float m_pullPower;
    [SerializeField] Transform m_handJoint;
    bool m_handsUp = false;
    Vector3 m_handRotation;
    Quaternion m_lookAtTarget;
    bool m_isEnergySource = false;

    private void OnEnable()
    {
        connectTime = new WaitForSeconds(0.2f);

    }


    void Update()
    {
        if (Connect)
            Disconnect();
        if (m_handsUp)
        {
            RotateHand();
        }

    }
    private void LateUpdate()
    {


    }



    void Disconnect()
    {
        if (Vector3.Distance(TargetObject.position, transform.position) > m_wireLength)
        {
            Connect = false;
        }
    }
    public void StartGrapple()
    {
        if (TargetObject == null)
            CheckSurroundings();
        if (TargetObject != null)
        {
            m_targetPoint = TargetObject.position;
            Source = TargetObject.parent.transform;
            m_handsUp = true;
            Connect = true;
            //PlayerMovementUpdate
            if (m_isEnergySource)
            {
                m_playerMovement.ConnectedToSource = true;
                m_playerMovement.TargetSource = Source;
            }



        }

    }



    public void StopGrapple()
    {
        StopAllCoroutines();
        if (joint != null) Destroy(joint);
        Connect = false;
        //PlayerMovementUpdate
        m_playerMovement.ConnectedToSource = false;
        m_playerMovement.TargetSource = null;
        m_isEnergySource = false;
        //
        TargetObject = null;
        m_handsUp = false;
    }




    public Vector3 GetTargetPoint()
    {
        return m_targetPoint;
    }


    private void RotateHand()
    {
        //m_handJoint.LookAt(TargetObject);
    }


    private void CheckSurroundings()
    {

        m_hitColliders = Physics.OverlapSphere(transform.parent.position + transform.parent.transform.up * 2, m_socketDetectionRange, m_interactableLayerMask);
        wireTag = IsHotWire ? "Socket_Hot" : "Socket_Neutral";
        foreach (var hitCollider in m_hitColliders)
        {
            if (hitCollider.gameObject.CompareTag(wireTag))
            {
                TargetObject = hitCollider.transform;
                m_handsUp = true;
                m_isEnergySource = true;
            }
            if (hitCollider.gameObject.CompareTag("Hook"))
            {

                TargetObject = hitCollider.transform;
                m_handsUp = true;
                Debug.Log("Hook Detected");
                Swing();


            }


        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position + transform.parent.up * 2, m_socketDetectionRange);
    }


    private void Swing()
    {
        if (joint == null)
        {

            StartCoroutine(HookSwing());


        }

    }

    IEnumerator HookSwing()
    {
        if (joint != null) Destroy(joint);
        yield return connectTime;
        if (joint != null) Destroy(joint);
        joint = m_playerMovement.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = TargetObject.position;

        float distanceFromPoint = Vector3.Distance(transform.position, TargetObject.position);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 1f;
        joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

    }

    public void PullPlayer()
    {
        if (joint != null)
        {
            if (joint.maxDistance > joint.minDistance)
                joint.maxDistance -= Time.deltaTime * m_pullPower;
        }

    }

}
