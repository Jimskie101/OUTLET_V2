using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class CheckpointManager : MonoBehaviour
{
    public int CurrentCheckpointID;
    
    [SerializeField] Transform m_playerTransform;




    [SerializeField] Transform []checkpoints;

    
    public void SetCurrentCheckpoint( int i_ID)
    {
        CurrentCheckpointID = i_ID;
    }

    [Button]
    public void LoadCurrentCheckpoint()
    {
        m_playerTransform.position = checkpoints[CurrentCheckpointID].position;
    }



}
