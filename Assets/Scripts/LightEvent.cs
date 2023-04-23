using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class LightEvent : MonoBehaviour
{
    [SerializeField] GameObject [] m_enableObjects;
    [SerializeField] Light [] m_targetLights;
    [SerializeField] float m_intensity;
    [SerializeField] float m_duration;
    [SerializeField] Ease m_lightEase;
    [SerializeField] bool m_realtime = true;
    
    [Button]
    public void LightUp()
    {
        foreach (GameObject g in m_enableObjects)
        {
            g.SetActive(true);
        }


        foreach (Light l in m_targetLights)
        {
            l.DOIntensity(m_intensity, m_duration).SetEase(m_lightEase).SetUpdate(m_realtime);
        }
    }
}
