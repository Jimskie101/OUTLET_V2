using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tasks", menuName = "ScriptableObjects/Tasks", order = 1)]
public class Tasks : ScriptableObject
{
   [TextArea] public string [] TasksText;
}
