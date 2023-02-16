using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PuzzleHandler : MonoBehaviour
{

    [SerializeField] int[] m_IDOrder;
    [SerializeField] int[] m_IDActive;
    int m_IDCounter = 0;


    [SerializeField] PuzzlePorts[] puzzlePorts;

    [Button]
    private void AddToPuzzlePorts()
    {
        puzzlePorts = GetComponentsInChildren<PuzzlePorts>();
    }



    public void PuzzleChecker(int i_ID)
    {
        m_IDActive[m_IDCounter] = i_ID;
        m_IDCounter++;


        for (int i = 0; i < m_IDOrder.Length; ++i)
        {
            if (!(m_IDActive[i] == m_IDOrder[i]))
            {
                return;
            }
        }
        Debug.Log("graargarharhadrhaga");
        GetComponentInParent<Generator>().Fixed();



    }

    public void WrongCombo()
    {
        m_IDCounter = 0;
        for (int i = 0; i < m_IDActive.Length; i++)
        {
            m_IDActive[i] = 0;
        }

        foreach (PuzzlePorts p in puzzlePorts)
        {
            p.Reset();
        }

    }
}
