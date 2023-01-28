using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class TerrainGenerator : MonoBehaviour
{
    [ExecuteInEditMode]
    public int depth = 20;
    public int width = 256;
    public int height = 256; // Length

    public float scale = 20;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public int elementSpacing = 3;

    public Element[] elements;

    Terrain terrain;


    private void Start()
    {
        StartGeneration();

    }
    [Button]
    private void StartGeneration()
    {
        offsetX = Random.Range(0, 9999f);
        offsetY = Random.Range(0, 9999f);


        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

    }

    TerrainData GenerateTerrain(TerrainData terrainData) 
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights() 
    { 
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        { 
            for(int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);

            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y) 
    { 
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
    // private void TreeSpawner()
    // {
    //     for (int x = 0; x < width; x += elementSpacing)
    //     {
    //         for (int z = 0; z < height; z += elementSpacing)
    //         {
    //             Element element = elements[0];
    //             Vector3 position = new Vector3(x, 0f, z);
    //             Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
    //             Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
    //             Vector3 scale = Vector3.one * Random.Range(-0.75f, 1.25f);

    //             GameObject newElement = Instantiate(element.prefab);
    //             newElement.transform.SetParent(transform);
    //             newElement.transform.position = position + offset;
    //             newElement.transform.eulerAngles = rotation;
    //             newElement.transform.localScale = scale;
    //         }
        
    //     }





    // }

    [System.Serializable]
    public class Element 
    {
        public string name;
        public GameObject prefab;

    
    }
}
