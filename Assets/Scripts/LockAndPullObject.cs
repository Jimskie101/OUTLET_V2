using UnityEngine;

public class LockAndPullObject : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 2f;
    [SerializeField] private LayerMask lockLayer;
    [SerializeField] PlayerMovement m_playerMovement;

    [SerializeField] private Rigidbody m_objectToLock;
    private bool isLocked;


    public void LockAndUnlock()
    {
        if (isLocked)
        {
            UnlockObject();
        }
        else
        {
            LockObject();
        }
    }
    Vector3 offset;
    float distanceToPlayer;
    RaycastHit[] hits;
    bool collided = false;
    private void LateUpdate()
    {
        if (isLocked && m_objectToLock != null)
        {
            offset = transform.position + transform.forward * maxLockDistance - m_objectToLock.transform.position;
            offset.y = 0f;

            // Check the distance between the player and the locked object
            distanceToPlayer = Vector3.Distance(m_objectToLock.transform.position, transform.position);
            if (distanceToPlayer > maxLockDistance * 1.5f) // Set a threshold distance to automatically unlock the object
            {
                UnlockObject();
            }
            else
            {
                // Check for collisions
                hits = m_objectToLock.SweepTestAll(offset.normalized, offset.magnitude, QueryTriggerInteraction.Ignore);
                collided = false;
                foreach (RaycastHit hit in hits)
                {
                    if (!hit.collider.isTrigger)
                    {
                        collided = true;
                        break;
                    }
                }

                // Move the object if there is no collision
                if (!collided)
                {
                    m_objectToLock.MovePosition(m_objectToLock.transform.position + offset);
                }
            }
        }
    }
    RaycastHit hit;
    private void LockObject()
    {
        if (Physics.Raycast(transform.position, transform.forward + -transform.up / 1.5f, out hit, maxLockDistance, lockLayer))
        {
            m_objectToLock = hit.rigidbody;
            m_objectToLock.isKinematic = true;
            isLocked = true;
            m_playerMovement.LockRotation = true;
            m_playerMovement.Holding = true;
        }
    }

    public void UnlockObject()
    {
        if (m_objectToLock != null)
        {
            m_objectToLock.isKinematic = false;
            m_objectToLock = null;
        }

        isLocked = false;
        m_playerMovement.LockRotation = false;
        m_playerMovement.Holding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward + -transform.up / 1.5f);
    }
}
