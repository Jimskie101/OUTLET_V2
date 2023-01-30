using UnityEngine;

public class Pusher : MonoBehaviour
{
    private CharacterController m_characterController;
    private Vector3 m_pushDirection = Vector3.zero;
    private float m_pushForce = 10f;

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

        m_pushDirection.x = hit.moveDirection.x * m_pushForce;
        m_pushDirection.z = hit.moveDirection.z * m_pushForce;

        m_rigidbody.AddForce(m_pushDirection);
    }
}
