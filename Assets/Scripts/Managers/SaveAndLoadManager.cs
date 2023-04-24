using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using System.IO;
using DG.Tweening;

//Custom Classes
[System.Serializable]
public class EntityData
{
    public int id;
    public string name;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class PlayerStatus
{
    public float lifeValue;
    public Vector3 position;
    public Quaternion rotation;
    public CameraPosition camPosition;
}

[System.Serializable]
public class SavedGameData
{
    public PlayerStatus playerStatus;
    public List<EntityData> entityData;
    public StageData stageData;
}

[System.Serializable]
public class StageData
{
    public int SceneNumber;
    public int CheckpointActive;
    public int WaypointActive;
    public int TaskActive;
    public int ObjectiveActive;
    public int CollectibleActive;
    public int PostersActive;
}







public class SaveAndLoadManager : MonoBehaviour
{
    SavedGameData m_savedGameData;
    SavedGameData m_loadedSaveData;
    public SavedGameData CheckGameData;
    string m_jsonRead;
    string m_filePathRead;
    string m_jsonWrite;
    string m_filePathWrite;
    string m_jsonCheck;
    string m_filePathCheck;
    [SerializeField] GameObject m_player;
    [SerializeField] PlayerStatus m_playerStatus;
    [SerializeField] CharacterController m_playerCC;
    PlayerScript m_playerScript;

    private void Awake()
    {
        try
        {
            m_player = FindObjectOfType<PlayerScript>().gameObject;
            if (m_player != null)
            {
                m_playerScript = m_player.GetComponent<PlayerScript>();
                m_playerCC = m_player.GetComponent<CharacterController>();
            }

        }
        catch
        {
            Debug.Log("There's no player in the scene!");
        }



    }
    private void Start()
    {
        if (m_player != null)
        {
            SceneStart();
        }


    }
    private void SceneStart()
    {
        if (Managers.Instance.GameData.LoadingFromSave)
        {

            LoadData();
            Managers.Instance.GameData.LoadingFromSave = false;

        }
        else if (GetCheckpointSceneNumber() != Managers.Instance.SceneHandler.GetCurrentScene())
        {
            Managers.Instance.CheckpointManager.SetCurrentCheckpoint(0);
        }
        
    }




    public bool CheckJson()
    {
        m_filePathCheck = Path.Combine(Application.persistentDataPath, "OutletGameData");
        if (File.Exists(m_filePathCheck))
        {
            m_jsonCheck = File.ReadAllText(m_filePathCheck);
            CheckGameData = JsonUtility.FromJson<SavedGameData>(m_jsonCheck);
            if (CheckGameData != null)
            {
                Debug.Log("There's a savefile!");
                return true;
            }
            else
            {
                Debug.Log("There's no savefile found!");
                return false;
            }
        }
        else return false;

    }
    private void FetchFromJson()
    {
        m_filePathRead = Path.Combine(Application.persistentDataPath, "OutletGameData");
        m_jsonRead = File.ReadAllText(m_filePathRead);
        m_loadedSaveData = JsonUtility.FromJson<SavedGameData>(m_jsonRead);
        m_playerStatus = m_loadedSaveData.playerStatus;
        EntityDatas = m_loadedSaveData.entityData;
        StageData = m_loadedSaveData.stageData;
    }

    private void SaveToJson()
    {
        m_jsonWrite = JsonUtility.ToJson(m_savedGameData);
        // Save the JSON string to a file
        m_filePathWrite = Path.Combine(Application.persistentDataPath, "OutletGameData");
        File.WriteAllText(m_filePathWrite, m_jsonWrite);

        Debug.Log("Saved data to " + m_filePathWrite);

    }
    public void ClearJson()
    {
        m_filePathWrite = Path.Combine(Application.persistentDataPath, "OutletGameData");
        File.WriteAllText(m_filePathWrite, "");
    }


    private PlayerStatus GetPlayerStatus()
    {
        m_playerStatus = new PlayerStatus();
        m_playerStatus.lifeValue = m_playerScript.LifePercentage;
        m_playerStatus.position = m_player.transform.localPosition;
        m_playerStatus.rotation = m_playerScript.transform.localRotation;
        m_playerStatus.camPosition = Managers.Instance.CameraHandler.CamPosition;
        return m_playerStatus;

    }
    private void LoadPlayerStatus()
    {
        m_playerScript.FallingDisabled = true;
        Debug.Log(m_playerStatus.position);
        m_playerCC.enabled = false;
        m_player.transform.localPosition = m_playerStatus.position;
        m_playerCC.enabled = true;
        m_playerScript.transform.localRotation = m_playerStatus.rotation;
        //Managers.Instance.CameraHandler.CamPosition = m_playerStatus.camPosition;
        Managers.Instance.GameManager.ChangeGameDirection();
        DOVirtual.DelayedCall(1f, () => m_playerScript.FallingDisabled = false);
        StartCoroutine(LifeUpdater());
    }

    public StageData StageData;
    private StageData GetStageData()
    {
        StageData = new StageData();
        StageData.SceneNumber = Managers.Instance.SceneHandler.GetCurrentScene();
        StageData.CheckpointActive = Managers.Instance.CheckpointManager.CurrentCheckpointID;
        StageData.TaskActive = Managers.Instance.TaskManager.TaskNumber;
        StageData.WaypointActive = Managers.Instance.WaypointManager.WaypointCounter;
        StageData.ObjectiveActive = Managers.Instance.GameManager.ObjectiveCounter;
        StageData.CollectibleActive = Managers.Instance.GameManager.CollectibleCount;
        StageData.PostersActive = Managers.Instance.GameManager.PostersCount;
        return StageData;
    }





    IEnumerator LifeUpdater()
    {
        yield return new WaitForSeconds(0.01f);
        m_playerScript.LifePercentage = m_playerStatus.lifeValue;
    }


    public List<GameObject> Entities;
    public List<EntityData> EntityDatas;

    EntityData m_entityData;

    private List<EntityData> GetEntitiesData()
    {
        EntityDatas.Clear();
        foreach (GameObject g in Entities)
        {
            m_entityData = new EntityData();
            m_entityData.id = Entities.IndexOf(g);
            m_entityData.name = g.name;
            m_entityData.position = g.transform.localPosition;
            m_entityData.rotation = g.transform.rotation;
            EntityDatas.Add(m_entityData);
        }
        return EntityDatas;
    }



    [Button]
    public void SaveData()
    {

        Managers.Instance.UIManager.ShowGameUpdate("Checkpoint Reached");
        m_savedGameData = new SavedGameData();
        m_savedGameData.stageData = GetStageData();
        m_savedGameData.playerStatus = GetPlayerStatus();
        m_savedGameData.entityData = GetEntitiesData();




        SaveToJson();


    }

    public int GetCheckpointSceneNumber()
    {
        FetchFromJson();
        return StageData.SceneNumber;

    }


    [Button]
    public void LoadData()
    {

        FetchFromJson();


        LoadPlayerStatus();
        foreach (GameObject g in Entities)
        {
            g.transform.localPosition = EntityDatas[Entities.IndexOf(g)].position;
            g.transform.rotation = EntityDatas[Entities.IndexOf(g)].rotation;

        }

        Managers.Instance.WaypointManager.WaypointCounter = StageData.WaypointActive - 1 < 0 ? -1 : StageData.WaypointActive - 1;
        Managers.Instance.TaskManager.TaskNumber = StageData.TaskActive;
        Managers.Instance.GameManager.ObjectiveCounter = StageData.ObjectiveActive;
        Managers.Instance.GameManager.CollectibleCount = StageData.CollectibleActive;
        Managers.Instance.CollectibleManager.CollectedItems = StageData.CollectibleActive;
        Managers.Instance.GameManager.PostersCount = StageData.PostersActive;
        Managers.Instance.CollectibleManager.Posters = StageData.PostersActive;
        m_player.transform.localPosition = m_playerStatus.position;
    }













}
