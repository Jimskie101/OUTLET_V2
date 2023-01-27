using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }
    public UIManager UIManager;
    public SceneHandler SceneHandler;
    public CameraHandler CameraHandler;
    public CollectibleManager CollectibleManager;
    public InputHandler InputHandler;

    private void Awake() 
{ 
    // If there is an instance, and it's not me, delete myself.
    
    if (Instance != null && Instance != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        Instance = this; 
    } 
}
}
