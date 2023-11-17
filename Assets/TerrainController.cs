using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField] float scale;
    [SerializeField] float terrainHeightMultiplier;
    [SerializeField] AnimationCurve heightCurve;

    [SerializeField] GameObject[] smallObjectsPrefabs;
    [SerializeField] int numberOfSmallObjects;
    [SerializeField] GameObject[] bigObjectsPrefabs;
    [SerializeField] int numberOfBigObjects;

    private Terrain terrain;
    private TerrainCollider terrainCollider;
    private Transform terrainHolderTransform;

    private void Start()
    {
        terrainHolderTransform = new GameObject("TerrainHolder").transform;
        terrain = GetComponent<Terrain>();
        terrainCollider = GetComponent<TerrainCollider>();

        GenerateTerrain();
        RandomizeSmallObjects();
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

    void RandomizeSmallObjects()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        for (int i = 0; i < numberOfSmallObjects; i++)
        {
            float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
            float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

            Vector3 smallObjPos = new Vector3(randomX, terrainPosition.y + terrainSize.y, randomZ);

            Ray ray = new Ray(smallObjPos + Vector3.up , Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GroundPlane")))
            {
                smallObjPos.y = hit.point.y;

                int randomTreeIndex = Random.Range(0, smallObjectsPrefabs.Length);
                GameObject smallObjPrefab = smallObjectsPrefabs[randomTreeIndex];

                Instantiate(smallObjPrefab, smallObjPos, Quaternion.identity, terrainHolderTransform);
            }
        }
    }



}
