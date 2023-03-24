using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Main Settings")]
    [SerializeField] bool m_loadSave = false;

    public bool LoadingFromSave
    {
        get { return m_loadSave; }
        set { m_loadSave = m_lockGameData ? true : value; }
    }
    [Space]
    [SerializeField] bool m_lockGameData = false;
    public int NextSceneIndex = 0;
}
