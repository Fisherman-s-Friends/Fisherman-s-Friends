using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    private Terrain terrain;
    private TerrainCollider terrainCollider;

    public AnimationCurve heightCurve;

    public float scale = 20f;
    public float terrainHeightMultiplier = 10f;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainCollider = GetComponent<TerrainCollider>();

        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        if (terrain == null || terrainCollider == null) { return; }

        TerrainData terrainData = terrain.terrainData;
        terrainData = GenerateTerrainMesh(terrainData);
        terrain.terrainData = terrainData;
        terrainCollider.terrainData = terrainData;
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