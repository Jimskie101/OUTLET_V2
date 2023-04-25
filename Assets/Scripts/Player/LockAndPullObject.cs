using UnityEngine;

public class LockAndPullObject : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 2f;
    [SerializeField] private LayerMask lockLayer;
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] Transform m_objectHolder;

    [SerializeField] private Rigidbody m_objectToLock;
    Transform m_defaultParent;
    [HideInInspector]public bool IsLocked;


    public void LockAndUnlock()
    {
        if (IsLocked)
        {
            UnlockObject();
        }
        else
        {
            LockObject();
        }
    }
    float distanceToPlayer;

    private void LateUpdate()
    {

        if (IsLocked && m_objectToLock != null)
        {
            distanceToPlayer = Vector3.Distance(m_objectToLock.transform.position, transform.position);
            if (distanceToPlayer > maxLockDistance) // Set a threshold distance to automatically unlock the object
            {
                UnlockObject();
            }
        }
    }


    // private void LateUpdate()
    // {
    //     if (isLocked && m_objectToLock != null)
    //     {
    //         offset = transform.position + transform.forward * maxLockDistance - m_objectToLock.transform.position;
    //         offset.y = 0f;

    //         // Check the distance between the player and the locked object
    //         
    //         else
    //         {
    //             // Check for collisions
    //             hits = m_objectToLock.SweepTestAll(offset.normalized, offset.magnitude, QueryTriggerInteraction.Ignore);
    //             collided = false;
    //             foreach (RaycastHit hit in hits)
    //             {
    //                 if (!hit.collider.isTrigger)
    //                 {
    //                     collided = true;
    //                     break;
    //                 }
    //             }

    //             // Move the object if there is no collision
    //             if (!collided)
    //             {
    //                 m_objectToLock.MovePosition(m_objectToLock.transform.position + offset);
    //             }
    //         }
    //     }
    // }
    RaycastHit hit;
    private void LockObject()
    {
        if (Physics.Raycast(transform.position, transform.forward + -transform.up / 1.5f, out hit, maxLockDistance, lockLayer))
        {
            m_objectToLock = hit.rigidbody;
            m_defaultParent = m_objectToLock.transform.parent;
            m_objectToLock.transform.SetParent(m_objectHolder);
            IsLocked = true;
            m_playerMovement.LockRotation = true;
            m_playerMovement.Holding = true;
        }
    }

    public void UnlockObject()
    {
        if (m_objectToLock != null)
        {
            m_objectHolder.DetachChildren();
            m_objectToLock.transform.SetParent(m_defaultParent);

            m_objectToLock = null;

        }

        IsLocked = false;
        m_playerMovement.LockRotation = false;
        m_playerMovement.Holding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward + -transform.up / 1.5f);
    }
}
