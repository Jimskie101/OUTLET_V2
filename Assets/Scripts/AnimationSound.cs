using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    AudioManager m_audioManager;
    [SerializeField] bool m_fromObject = false;
    private void Start()
    {
        m_audioManager = Managers.Instance.AudioManager;
    }
    public void PlayThisAnimationSound(string animationName)
    {
        m_audioManager.PlayHere(animationName, this.gameObject, m_fromObject);
    }

}
