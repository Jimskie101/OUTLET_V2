using UnityEngine;

public class LockAndPullObject : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 2f;
    [SerializeField] private LayerMask lockLayer;
    [SerializeField] PlayerMovement m_playerMovement;

    private Rigidbody objectToLock;
    private bool isLocked;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
    }
    Vector3 offset;
    float distanceToPlayer;
    RaycastHit[] hits;
    bool collided = false;
    private void LateUpdate()
    {
        if (isLocked && objectToLock != null)
    {
        offset = transform.position + transform.forward * maxLockDistance - objectToLock.transform.position;
        offset.y = 0f;

        // Check the distance between the player and the locked object
        distanceToPlayer = Vector3.Distance(objectToLock.transform.position, transform.position);
        if (distanceToPlayer > maxLockDistance * 1.5f) // Set a threshold distance to automatically unlock the object
        {
            UnlockObject();
        }
        else
        {
            // Check for collisions
            hits = objectToLock.SweepTestAll(offset.normalized, offset.magnitude);
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
                objectToLock.MovePosition(objectToLock.transform.position + offset);
            }
        }
    }
    }
    RaycastHit hit;
    private void LockObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxLockDistance, lockLayer))
        {
            objectToLock = hit.rigidbody;
            objectToLock.isKinematic = true;
            isLocked = true;
            m_playerMovement.LockRotation = true;
        }
    }

    private void UnlockObject()
    {
        objectToLock.isKinematic = false;
        objectToLock = null;
        isLocked = false;
        m_playerMovement.LockRotation = false;
    }
}
