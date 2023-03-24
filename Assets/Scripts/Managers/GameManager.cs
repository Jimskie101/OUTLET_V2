using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
public class GameManager : MonoBehaviour
{
    public GameObject Player;

    [Header("Tools and Cheats")]
    //Cheats and Tools
    public bool NoDeathMode = false;
    public bool UnliLight = false;
    public bool NoCutscene = false;
    
    public int ObjectiveCounter = 0;
  
    public int CollectibleCount = 0; 
    public int PostersCount = 0; 


    public enum Direction
    {
        front,
        left,
        back,
        right,
    }
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
}
