using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData : ScriptableObject
{
    [Header("PowerUp Settings")]
    [Header("Jump PowerUp")]
    public float JumpBoostDuration;
    public float JumpAddedValue;

    [Header("Shield PowerUp")]
    public float ShieldDuration;

     [Header("LightLock PowerUp")]
    public float LightLockDuration;

    

}
