using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Engaged : MonoBehaviour
{
   Rigidbody m_rb;
    [SerializeField] float m_duration;
    [SerializeField] Vector3 m_targetPosition;
    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }
    [Button]
    public void Engage()
    {
        m_rb.isKinematic = true;
        Managers.Instance.GameManager.Player.GetComponent<LockAndPullObject>().UnlockObject();
        LockMe();
    }
    
    public void LockMe()
    {
        gameObject.layer = LayerMask.NameToLayer("Environment");
        transform.DOLocalMove(transform.position - m_targetPosition, m_duration).SetUpdate(true);
    }
}
