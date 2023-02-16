using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager m_checkpointManager;
    [SerializeField] int m_checkpointID;

    private void Start()
    {
        m_checkpointManager = Managers.Instance.CheckpointManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_checkpointManager.CurrentCheckpointID < m_checkpointID )
            {
                m_checkpointManager.SetCurrentCheckpoint(m_checkpointID);
            }
        }

    }
}
