using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ColorChanger : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer m_playerBodyRenderer;
    [SerializeField] Material  m_gold;
    [SerializeField] Material []m_newMaterials;
    [SerializeField] Material [] m_defaultMaterials;
    bool m_initialized = false;

    private void OnEnable()
    {
        if(!m_initialized)
        {
            m_defaultMaterials = m_playerBodyRenderer.materials;
            m_initialized = true;
        }
        Array.Copy(m_defaultMaterials, m_newMaterials, m_defaultMaterials.Length);
        m_newMaterials[0] = m_gold;
        Debug.Log("Changed Color",this.gameObject);
        m_playerBodyRenderer.materials = m_newMaterials;
    }
    private void OnDisable()
    {
        Debug.Log("Reset Color",this.gameObject);
        m_playerBodyRenderer.materials = m_defaultMaterials;
    }
   

}
