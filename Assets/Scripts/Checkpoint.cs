using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager m_checkpointManager;
    public int CheckpointID;

    private void Start()
    {
        m_checkpointManager = Managers.Instance.CheckpointManager;
        CheckpointID = int.Parse(name);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_checkpointManager.CurrentCheckpointID < CheckpointID )
            {
                m_checkpointManager.SetCurrentCheckpoint(CheckpointID);
                GetComponent<BoxCollider>().enabled = false;
                this.gameObject.SetActive(false);
            }
        }

    }
}
