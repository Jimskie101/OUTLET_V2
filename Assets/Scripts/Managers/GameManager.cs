using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using DG.Tweening;


public enum Direction
{
    front,
    left,
    back,
    right,
}
public class GameManager : MonoBehaviour
{
    public GameObject Player;
    private PlayerScript m_playerScript;
    private CharacterController m_playerCC;
    private void Awake()
    {


        if (!Application.isEditor)
        {
            NoDeathMode = false;
             UnliLight = false;
        }
        try
        {
            Player = FindObjectOfType<PlayerScript>().gameObject;
            if (Player != null)
            {
                m_playerScript = Player.GetComponent<PlayerScript>();
                m_playerCC = Player.GetComponent<CharacterController>();
            }

        }
        catch
        {
            Debug.Log("There's no player in the scene!");
        }
    }

    [Header("Tools and Cheats")]
    //Cheats and Tools
    public bool NoDeathMode = false;
    public bool UnliLight = false;
    public bool NoCutscene = false;

    public int ObjectiveCounter = 0;

    public int CollectibleCount = 0;
    public int PostersCount = 0;



    [Header("Game Camera Direction")]
    public Direction GameDirection;
    public Vector3 XOrientation;
    public Vector3 ZOrientation;

    //Changes the game control direction
    [Button]
    public void ChangeGameDirection()
    {
        switch (GameDirection)
        {
            case Direction.front:
                XOrientation = Vector3.right;
                ZOrientation = Vector3.forward;
                break;

            case Direction.left:
                XOrientation = -Vector3.forward;
                ZOrientation = Vector3.right;
                break;

            case Direction.back:
                XOrientation = -Vector3.right;
                ZOrientation = -Vector3.forward;
                break;

            case Direction.right:
                XOrientation = Vector3.forward;
                ZOrientation = -Vector3.right;
                break;
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Debug.Log("Timescale Reset");
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Debug.Log("Timescale is 2");
            Time.timeScale = 2f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            TeleportPlayerToNextWaypoint();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            UnliLight = !UnliLight;
            Managers.Instance.UIManager.ShowGameUpdate(UnliLight ? "Cheat Activated\nUnliLight" : "Cheat Deactivated\nUnliLight");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            NoDeathMode = !NoDeathMode;
            Managers.Instance.UIManager.ShowGameUpdate(NoDeathMode ? "Cheat Activated\nNoDeath" : "Cheat Deactivated\nNoDeath");
        }
    }

    [Button]
    private void TeleportPlayerToNextWaypoint()
    {
        m_playerScript.FallingDisabled = true;
        Debug.Log("Teleported");
        m_playerCC.enabled = false;
        Player.transform.position = Managers.Instance.WaypointManager.NextPosition();
        m_playerCC.enabled = true;

        DOVirtual.DelayedCall(1f, () => m_playerScript.FallingDisabled = false);
    }

}
