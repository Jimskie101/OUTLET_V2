using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;


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
    private void Awake()
    {
        Player = FindObjectOfType<PlayerScript>().gameObject;
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
    }

    [Button]
    private void TeleportPlayerToNextWaypoint()
    {
        Player.transform.position = Managers.Instance.WaypointManager.NextPosition();
    }
}
