using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject cube;
    public float spawnHeight = 2.0f;
    public float spawnRange = 5.0f;
    [SerializeField] bool keepRunning = false;

    [Button]
    void Run()
    {
        keepRunning = !keepRunning;
    }
     [Button]
    void SpawnObject()
    {
        // Get the size and center of the cube
        Bounds bounds = cube.GetComponent<Renderer>().bounds;
        Vector3 size = bounds.size;
        Vector3 center = bounds.center;

        // Find a suitable spawn position inside the bounds of the cube
        Vector3 spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2),
                                                spawnHeight,
                                                Random.Range(-size.z / 2, size.z / 2));

        int index = Random.Range(0, prefabs.Length);

        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Debug.Log("Hit " + hit.point);
            spawnPos.y = hit.point.y + spawnHeight;
            // Check if there's another collider at the spawn position
            Collider[] colliders = Physics.OverlapSphere(spawnPos, 1.0f, LayerMask.GetMask("Environment"));
            if (colliders.Length > 0)
            {
                // If there's another collider, try again in the next frame
                Invoke("SpawnObject", 0.01f);
            }
            else
            {
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(prefabs[index], spawnPos, randomRotation);
                if(keepRunning)
                Invoke("SpawnObject", 0.01f);
            }
        }


    }
}
