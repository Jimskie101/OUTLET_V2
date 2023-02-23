using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireBase : MonoBehaviour
{
    [SerializeField] PlayerData m_playerData;
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] Animator m_playerAnimator;
    [SerializeField] Vector3 m_targetPoint;
    [SerializeField] LayerMask m_interactableLayerMask;
    public Transform TargetObject = null;
    public Transform Source = null;
    Vector3 m_lookPosition;
    public bool Connect = false;
    public bool IsHotWire = false;
    Collider[] m_hitColliders;
    private SpringJoint m_joint;
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

    public void PullPlayer()
    {

        if (m_joint != null)
            if (m_joint.maxDistance > m_joint.minDistance)
            {

                m_joint.maxDistance -= Time.deltaTime * m_pullPower;

            }

    }


    void Disconnect()
    {
        if (Vector3.Distance(TargetObject.position, transform.position) > m_playerData.WireLength)
        {
            Connect = false;
            if (m_joint != null)
            {
                // Destroy(joint);
                ResetFakeRigidBody();

            }

        }
    }
    public void StartGrapple()
    {
        if (TargetObject == null)
            CheckSurroundings();
        if (TargetObject != null)
        {
            m_targetPoint = TargetObject.position;

            m_handsUp = true;
            Connect = true;
            //PlayerMovementUpdate
            if (m_isEnergySource)
            {
                Source = TargetObject.parent.transform;
                m_playerMovement.ConnectedToSource = true;
                m_playerMovement.TargetSource = Source;
            }



        }

    }



    public void StopGrapple()
    {
        StopAllCoroutines();
        if (m_joint != null)
        {
            // Destroy(joint);
            ResetFakeRigidBody();

        }
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

        m_hitColliders = Physics.OverlapSphere(transform.parent.position + transform.parent.transform.up * 2, m_playerData.WireRangeRadius, m_interactableLayerMask);
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
        Gizmos.DrawWireSphere(transform.parent.position + transform.parent.up * 2, m_playerData.WireRangeRadius);
    }


    private void Swing()
    {

        StartCoroutine(HookSwing());



    }

    GameObject m_tempObj;
    CharacterController m_tempCc;
    IEnumerator HookSwing()
    {

        if (m_tempObj == null)
            m_tempObj = m_playerMovement.FakeRigidBody;
        m_tempObj.SetActive(true);
        m_tempObj.transform.position = m_playerMovement.transform.position;
        m_tempObj.transform.rotation = m_playerMovement.transform.rotation;
        if (m_tempCc == null)
            m_tempCc = m_playerMovement.GetComponent<CharacterController>();
        m_tempCc.enabled = false;
        m_playerMovement.transform.SetParent(m_playerMovement.FakeRigidBody.transform);




        yield return connectTime;
        //if (joint != null) Destroy(joint);
        if (!m_tempObj.TryGetComponent(out m_joint))
            m_joint = m_tempObj.AddComponent<SpringJoint>();
        m_joint.autoConfigureConnectedAnchor = false;
        m_joint.connectedAnchor = TargetObject.position;

        m_playerAnimator.SetBool("swinging", true);
        
        float distanceFromPoint = Vector3.Distance(transform.position, TargetObject.position);

        //The distance grapple will try to keep from grapple point. 
        m_joint.maxDistance = distanceFromPoint * 1f;
        m_joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        m_joint.spring = 4.5f;
        m_joint.damper = 7f;
        m_joint.massScale = 4.5f;

    }


    private void ResetFakeRigidBody()
    {
        m_playerAnimator.SetBool("swinging", false);

        m_tempObj.SetActive(false);
        m_tempObj.transform.DetachChildren();
        m_tempCc.enabled = true;

        m_playerMovement.transform.rotation = Quaternion.Euler(0, m_playerMovement.transform.rotation.y, 0);

    }

}
