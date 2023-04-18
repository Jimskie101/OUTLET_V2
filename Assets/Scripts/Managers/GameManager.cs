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
    private void Awake()
    {
        Player = FindObjectOfType<PlayerScript>().gameObject;
        m_playerScript = Player.GetComponent<PlayerScript>();
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



    private void Update() {
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            Debug.Log("Timescale Reset");
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(KeyCode.Period))
        {
            Debug.Log("Timescale is 2");
            Time.timeScale = 2f;
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
           UnliLight = !UnliLight;
        }
         if(Input.GetKeyDown(KeyCode.Alpha0))
        {
           NoDeathMode = !NoDeathMode;
        }
    }

    [Button]
    private void TeleportPlayerToNextWaypoint()
    {
        m_playerScript.FallingDisabled = true;
        Player.transform.position = Managers.Instance.WaypointManager.NextPosition();
        DOVirtual.DelayedCall(1f, () => m_playerScript.FallingDisabled = false);
    }
}
