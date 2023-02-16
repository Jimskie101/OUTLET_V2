using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class SaveAndLoadManager : MonoBehaviour
{
    public void CheckpointSave(int i_checkpointID)
    {
        PlayerPrefs.SetInt("CurrentCheckpoint", i_checkpointID);
        Debug.Log("Game Saved");
    }

    public int CheckpointLoad()
    {
        Debug.Log("Game Load");
        return PlayerPrefs.GetInt("CurrentCheckpoint");
    }

}
