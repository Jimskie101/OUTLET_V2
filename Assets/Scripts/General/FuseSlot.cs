using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class FuseSlot : MonoBehaviour
{   
    
    [SerializeField] Vector3 m_slotPosition;
    [SerializeField] UnityEvent m_action;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            Managers.Instance.GameManager.Player.GetComponent<LockAndPullObject>().UnlockObject();
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.DOLocalMove(m_slotPosition, 2f);
            other.transform.DOLocalRotate(new Vector3(90, 90, 0), 2f);
            other.tag = "Environment";
            GetComponent<BoxCollider>().enabled = false;
            //transform.parent.parent.GetComponentInChildren<TriggerToEvent>().Called();
            m_action.Invoke();
        }
    }
}
