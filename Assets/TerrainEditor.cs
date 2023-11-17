using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    private Terrain terrain;
    private TerrainCollider terrainCollider;

    public AnimationCurve heightCurve;

    public float scale = 20f;
    public float terrainHeightMultiplier = 10f;

    public List<GameObject> treePrefabs = new List<GameObject>();
    public int treeCount = 500;

    private void Start()
    {
        GenerateTerrain();
        GenerateFoliage();
    }

    void GenerateTerrain()
    {
        terrain = GetComponent<Terrain>();
        terrainCollider = GetComponent<TerrainCollider>();

        if (terrain == null || terrainCollider == null) { return; }

        TerrainData terrainData = terrain.terrainData;
        terrainData = GenerateTerrainMesh(terrainData);
        terrain.terrainData = terrainData;
        terrainCollider.terrainData = terrainData;
    }

    void GenerateFoliage()
    {
        Terrain terrain = GetComponent<Terrain>();

        if (terrain == null || treePrefabs.Count == 0) { return; }

        terrain.terrainData.treePrototypes = new TreePrototype[treePrefabs.Count];

        for (int i = 0; i < treePrefabs.Count; i++)
        {
            TreePrototype treePrototype = new TreePrototype();
            treePrototype.prefab = treePrefabs[i];
            terrain.terrainData.treePrototypes[i] = treePrototype;
        }

        TreeInstance[] treeInstances = new TreeInstance[treeCount];
        for (int i = 0; i < treeCount; i++)
        {
            TreeInstance treeInstance = new TreeInstance();
            treeInstance.position = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
            treeInstance.position.y = terrain.terrainData.GetHeight(Mathf.RoundToInt(treeInstance.position.x * (terrain.terrainData.heightmapResolution - 1)), Mathf.RoundToInt(treeInstance.position.z * (terrain.terrainData.heightmapResolution - 1)));

            treeInstance.position.Scale(terrain.terrainData.size);
            treeInstance.prototypeIndex = Random.Range(0, treePrefabs.Count); 

            treeInstances[i] = treeInstance;
        }

        terrain.terrainData.treeInstances = treeInstances;
        terrain.Flush();
    }

    TerrainData GenerateTerrainMesh(TerrainData terrainData)
    {
        int width = terrainData.heightmapResolution - 1;
        int length = terrainData.heightmapResolution - 1;

        terrainData.size = new Vector3(terrainData.size.x, terrainHeightMultiplier, terrainData.size.z);
        terrainData.SetHeights(0, 0, GenerateHeights(width, length));
        return terrainData;
    }

    float[,] GenerateHeights(int width, int length)
    {
        float[,] heights = new float[width, length];
        Vector2 offset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = CalculateHeight(x, y, offset);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y, Vector2 offset)
    {
        float xCoord = (float)x / (terrain.terrainData.heightmapResolution - 1) * scale + offset.x;
        float yCoord = (float)y / (terrain.terrainData.heightmapResolution - 1) * scale + offset.y;

        return heightCurve.Evaluate(Mathf.PerlinNoise(xCoord, yCoord));
    }
}