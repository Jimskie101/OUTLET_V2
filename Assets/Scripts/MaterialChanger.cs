using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using DG.Tweening;

public class MaterialChanger : MonoBehaviour
{
    Material m_targetMaterial;
    [SerializeField] int m_materialIndex;
    [SerializeField] float m_delayTime = 0;
    private void Start()
    {
        m_targetMaterial = GetComponent<MeshRenderer>().materials[m_materialIndex];
    }

    float startValue = 0;
    [Button]
    public void EnableEmission()
    {
        DOVirtual.DelayedCall(m_delayTime, () => EmissionOn());
    }
    private void EmissionOn()
    {
        m_targetMaterial.EnableKeyword("_EMISSION");
    }
    public void EmissionOff()
    {
        m_targetMaterial.DisableKeyword("_EMISSION");
    }
}
