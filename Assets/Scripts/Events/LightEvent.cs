using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;

public class LightEvent : MonoBehaviour
{
    [SerializeField] GameObject[] m_enableObjects;
    [SerializeField] Light[] m_targetLights;
    [SerializeField] float m_intensity;
    [SerializeField] float m_duration;
    [SerializeField] Ease m_lightEase;
    [SerializeField] bool m_realtime = true;


    [Button]
    public void LightUp()
    {
        if (sequence)
        {
            SequenceLight();
        }
        else
        {
            AllLight();
        }
    }

    private void AllLight()
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
    [Header("Sequence Lighting")]
    [SerializeField] bool sequence = false;
    [SerializeField] float lightInterval = 0f;
    int m_indexObj = 0;
    int m_indexLight = 0;
    private void SequenceLight()
    {

        // Check if we have reached the end of the sequence
        if (m_indexObj >= m_enableObjects.Length && m_indexLight >= m_targetLights.Length)
        {
            return;
        }

        // Get the current light and increase its intensity using DOTween
        if (m_indexObj != m_enableObjects.Length)
        {
            m_enableObjects[m_indexObj].SetActive(true);
            m_indexObj++;
        }
        if (m_indexLight != m_targetLights.Length)
        {
            m_targetLights[m_indexLight].DOIntensity(m_intensity, m_duration).SetEase(m_lightEase).SetUpdate(m_realtime);
            m_indexLight++;
        }
        // Call this function again after the specified delay
        Invoke("SequenceLight", lightInterval);
    }
}
