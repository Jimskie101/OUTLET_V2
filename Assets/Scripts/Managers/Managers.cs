using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }
    public GameManager GameManager;
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
    [Button]
    private void InitializeFields()
    {
        GameManager = GetComponentInChildren<GameManager>();
        UIManager = GetComponentInChildren<UIManager>();
        SceneHandler = GetComponentInChildren<SceneHandler>();
        CameraHandler = GetComponentInChildren<CameraHandler>();
        CollectibleManager = GetComponentInChildren<CollectibleManager>();
        InputHandler = GetComponentInChildren<InputHandler>();
    }
}
