using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    CollectibleManager m_collectibleManager;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    Vector3 m_rotation = new Vector3(0, 360f, 0);
    private void Start()
    {   
        disableDelay = new WaitForSeconds(0.5f);
        m_collectibleManager = Managers.Instance.CollectibleManager;
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.GetComponentInChildren<ParticleSystem>().Play();
            m_collectibleManager.CollectedSomething();
            StartCoroutine(Disabling());
        }

    }
    WaitForSeconds disableDelay;
    
    IEnumerator Disabling()
    {
        yield return disableDelay;
        this.gameObject.SetActive(false);
    }
}
