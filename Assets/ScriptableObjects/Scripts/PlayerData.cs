using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Player Movement Settings")]
    
    public float JumpHeight;
    public float Speed;
    public float RunSpeedMultiplier;
    public float FallThresholdLimit;
    public float RotationTime;
    public float GravityPullValue;
    public float GroundCheckSphereRadius;
    public LayerMask GroundLayer;



    [Header("Player Script Settings")]
    [Range(0,1)]
    public float IntialLifeValue;
    public float DimMultiplier;


    [Header("Player Pusher Settings")]
    public float PushForce;



}
