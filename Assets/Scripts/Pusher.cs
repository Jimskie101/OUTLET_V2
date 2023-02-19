using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] PlayerData m_playerData;
    private CharacterController m_characterController;
    private Vector3 m_pushDirection = Vector3.zero;

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody m_rigidbody = hit.collider.attachedRigidbody;
        

        if (m_rigidbody == null || m_rigidbody.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }
        
        m_pushDirection = hit.gameObject.transform.position - transform.position;
        m_pushDirection.y = 0f;
        m_pushDirection.Normalize();

        m_rigidbody.AddForceAtPosition(m_pushDirection * m_playerData.PushForce, transform.position, ForceMode.Impulse);
        
    }
}
