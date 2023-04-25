using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PuzzleIndicator : MonoBehaviour
{
    [SerializeField] MeshRenderer m_mesh;
    [SerializeField] bool m_active;
    [SerializeField] Material m_activeMAT;
    [SerializeField] Material m_inactiveMAT;
    [SerializeField] int m_ID;


    public void Activate()
    {
        if (!m_active)
        {
            m_mesh.sharedMaterial = m_activeMAT;
            m_active = true;
            // GetComponentInParent<PuzzleHandler>().PuzzleChecker(m_ID);
        }

    }
    public void Reset()
    {
        m_mesh.sharedMaterial = m_inactiveMAT;
        m_active = false;
    }



}
