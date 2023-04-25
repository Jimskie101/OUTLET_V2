using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Keypad : MonoBehaviour
{
    [SerializeField] Transform m_door;
    WaitForSeconds m_time;
    [SerializeField] Vector3 m_rotation = new Vector3(-90, 0, 255f);
    [SerializeField] ParticleSystem[] sparks;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_time = new WaitForSeconds(2f);
        Invoke("ObjectiveChecker", 0.1f);
    }
    bool m_done = false;

    [Button]
    private void FillFields()
    {
        sparks = GetComponentsInChildren<ParticleSystem>();
    }

    [Button]
    public void Activate()
    {

        StartCoroutine(ShortCirc());

    }
    IEnumerator ShortCirc()
    {
        yield return m_time;
        if (!m_done)
        {
            foreach (ParticleSystem ps in sparks)
            {
                ps.Play();
                Managers.Instance.AudioManager.PlayHere("spark", this.gameObject, true);
            }
            m_done = true;
            //Update the GameManager objectives
            Managers.Instance.GameManager.ObjectiveCounter++;
            Managers.Instance.CutsceneManager.PlayCutscene(1);

            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        yield return m_time;
        Managers.Instance.AudioManager.PlayHere("outpost_door", m_door.gameObject, true);
        Managers.Instance.TaskManager.NextTask();
        m_door.DOLocalRotate(m_rotation, 2f).SetUpdate(true);
        
    }


    //Must have for objectives
    [SerializeField] int m_forLoadingId;
    private void ForLoading()
    {
        Debug.Log("Forloading",this.gameObject);
        m_door.DOLocalRotate(m_rotation, 0f).SetUpdate(true);
    }
    private void ObjectiveChecker()
    {
        if (Managers.Instance.GameManager.ObjectiveCounter >= m_forLoadingId)
            ForLoading();
    }
}
