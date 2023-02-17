using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class Keypad : MonoBehaviour
{
    [SerializeField] Transform m_door;
    WaitForSeconds m_time;
    Vector3 m_rotation = new Vector3(-90, 0, 255f);
    [SerializeField] ParticleSystem [] sparks;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_time = new WaitForSeconds(2f);
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
            foreach(ParticleSystem ps in sparks)
            {
                ps.Play();
            }
            m_done = true;
            Managers.Instance.CutsceneManager.PlayCutscene(1);
            
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        yield return m_time;
        //Managers.Instance.AudioManager.PlayHere("outpost_door", m_door.gameObject);
        m_door.DOLocalRotate(m_rotation, 2f).SetUpdate(true);
    }
}
