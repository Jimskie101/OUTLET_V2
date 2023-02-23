using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RatData", menuName = "ScriptableObjects/RatData", order = 1)]
public class RatData : ScriptableObject
{
    [Header("Navmesh Agent Settings")]
    
    public float Speed = 3.5f;
    public float AngularSpeed = 120;
    public float Acceleration = 8;


    [Header("Rat AI Settings")]
    public Vector2 WanderRadius = new Vector2(2,5);
    public float AttackRange = 5;
    public float ChaseRange = 20;
    public float ActivityRange = 40;
    public float AttackForce = 10;
    public float AttackCooldown = 2;
    public float AttackDamage = 0.1f;
    public float DamageResetTime = 2;



}
