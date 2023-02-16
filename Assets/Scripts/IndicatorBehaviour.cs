using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class IndicatorBehaviour : MonoBehaviour
{
    [SerializeField] Material m_plain;
    [SerializeField] Material m_glow;
    [SerializeField] Renderer m_renderer;

  

    public void ColorActive()
    {
        m_renderer.sharedMaterial = m_glow;

    }
    public void ColorInactive()
    {
        m_renderer.sharedMaterial = m_plain;
    }
}
