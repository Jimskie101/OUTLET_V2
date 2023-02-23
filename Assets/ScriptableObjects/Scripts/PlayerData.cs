using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Player Movement Settings")]
    
    public float JumpHeight = 1.5f;
    public float Speed = 4;
    public float RunSpeedMultiplier = 2;
    public float FallThresholdLimit = 8.4f;
    public float RotationTime = 0.2f;
    public float GravityPullValue = -9.8f;
    public float GroundCheckSphereRadius = 0.31f;
    public LayerMask GroundLayer;



    [Header("Player Script Settings")]
    [Range(0,1)]
    public float IntialLifeValue = 1;
    public float DimMultiplier = 0.5f;


    [Header("Player Pusher Settings")]
    public float PushForce = 0.3f;

    [Header("Wire Settings")]
    public float WireRangeRadius = 5;
    public float WireLength = 10;

}
