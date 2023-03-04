using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class CheckpointManager : MonoBehaviour
{
    public int CurrentCheckpointID = 0;

    [SerializeField] Transform m_player;
    [SerializeField] Transform m_checkPointParent;



    [SerializeField] Checkpoint[] m_checkpoints;


    private void Start()
    {
        StartCoroutine(EnableCheckpoints());
    }
    IEnumerator EnableCheckpoints()
    {
        yield return new WaitForSeconds(1f);
        foreach (Checkpoint c in m_checkpoints)
        {
            CurrentCheckpointID = Managers.Instance.SaveAndLoadManager.StageData.CheckpointActive;
            if (Managers.Instance.SaveAndLoadManager.StageData.CheckpointActive < c.CheckpointID)
            {
                c.gameObject.SetActive(true);
                c.GetComponent<BoxCollider>().enabled = true;
            }


        }
    }



    [Button]
    private void AddAllCheckpoints()
    {
        m_checkpoints = m_checkPointParent.GetComponentsInChildren<Checkpoint>(includeInactive: true);
        foreach (Checkpoint c in m_checkpoints)
        {
            c.CheckpointID = int.Parse(c.name);
            c.GetComponent<BoxCollider>().enabled = false;
            c.gameObject.SetActive(false);
        }
    }

    public void SetCurrentCheckpoint(int i_ID)
    {
        CurrentCheckpointID = i_ID;
        Managers.Instance.SaveAndLoadManager.SaveData();

    }





}
