using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPathFinder : MonoBehaviour
{
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private float PathHeightOffset = 1.25f;
    [SerializeField]
    private float PathUpdateSpeed = 0.25f;

    private NavMeshTriangulation Triangulation;
    private Coroutine DrawPathCoroutine;

    private void Awake()
    {
        Triangulation = NavMesh.CalculateTriangulation();
    }

    private void Start()
    {
        SpawnNewObject();
    }

    public void SpawnNewObject()
    {
      
        if (DrawPathCoroutine != null)
        {
            StopCoroutine(DrawPathCoroutine);
        }

        DrawPathCoroutine = StartCoroutine(DrawPathToCollectable());
    }

    private IEnumerator DrawPathToCollectable()
    {
        WaitForSeconds Wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while (Target != null)
        {
            if (NavMesh.CalculatePath(Player.position, Target.position, NavMesh.AllAreas, path))
            {
                lr.positionCount = path.corners.Length;

                for (int i = 0; i < path.corners.Length; i++)
                {
                    lr.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
                }
            }
           

            yield return Wait;
        }
    }
}