using UnityEngine;

[System.Serializable]
public class WeightedPrefab
{
    public GameObject prefab;
    [Range(0, 10)]
    public float weight;
    public bool noEffects;
}
public class TerrainController : MonoBehaviour
{
    [SerializeField] float scale;
    [SerializeField] float terrainHeightMultiplier;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] int objectsDensity;
    [SerializeField] WeightedPrefab[] weightedPrefabs;

    private Terrain terrain;
    private TerrainCollider terrainCollider;
    private TerrainData terrainData;
    private Transform terrainHolderTransform;
    private Camera mainCamera;
    private Vector3 terrainPosition;
    private Vector3 terrainSize;

    private void Start()
    {
        terrainHolderTransform = new GameObject("TerrainHolder").transform;
        terrain = GetComponent<Terrain>();
        terrainCollider = GetComponent<TerrainCollider>();
        mainCamera = Camera.main;

        if (terrain == null || terrainCollider == null) { return; }

        GenerateTerrain();
        RandomizePrefabs();
    }

    public void GenerateTerrain()
    {
        terrainData = terrain.terrainData;
        terrainData = GenerateTerrainMesh(terrainData);
        terrain.terrainData = terrainData;
        terrainCollider.terrainData = terrainData;
        terrainPosition = terrain.transform.position;
        terrainSize = terrain.terrainData.size;
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

    void RandomizePrefabs()
    {
        float minX = terrainPosition.x / 3f;
        float maxX = (terrainPosition.x + terrainSize.x) / 3f;
        float minZ = terrainPosition.z;
        float maxZ = (terrainPosition.z + terrainSize.z) / 4.1f;
        float totalWeight = CalculateTotalWeight();

        for (int i = 0; i < objectsDensity; i++)
        {
            float randomValue = Random.value * totalWeight;
            float randomX = Random.Range(minX, maxX);
            float randomZ = WeightedSpawningTowardsCam(minZ, maxZ);

            Vector3 objectPos = new Vector3(randomX, terrainPosition.y + terrainSize.y, randomZ);
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(objectPos);

            if (viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f)
            {
                Ray ray = new Ray(objectPos + Vector3.up, Vector3.down);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GroundPlane")))
                    return;

                float prefabYOffset = 0.05f;

                objectPos.y = hit.point.y - prefabYOffset;
                int selectedIndex = SelectRandomPrefab(randomValue);
                GameObject objectPrefab = weightedPrefabs[selectedIndex].prefab;
                InstantiatePrefab(objectPrefab, objectPos, selectedIndex);
            }
        }
    }

    void InstantiatePrefab(GameObject objectPrefab, Vector3 objectPos, int selectedIndex)
    {
        GameObject newPrefab = Instantiate(objectPrefab, objectPos, Quaternion.identity, terrainHolderTransform);
        Debug.Log("Prefab for the environment was spawned!");

        if (weightedPrefabs[selectedIndex].noEffects) { return; }

        float multiplier = Random.Range(0.6f, 1.2f);
        newPrefab.transform.localScale *= multiplier;
        newPrefab.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    float WeightedSpawningTowardsCam(float min, float max)
    {
        if (Random.value < 0.70f)
            return Random.Range(min, max / Random.Range(2f, 3f));
        else
            return Random.Range(min, max);
    }

    float CalculateTotalWeight()
    {
        float totalWeight = 0f;

        foreach (var weightedPrefab in weightedPrefabs)
            totalWeight += weightedPrefab.weight;

        return totalWeight;
    }

    int SelectRandomPrefab(float randomValue)
    {
        int selectedIndex = -1;
        float cumulativeWeight = 0f;

        for (int j = 0; j < weightedPrefabs.Length; j++)
        {
            cumulativeWeight += weightedPrefabs[j].weight;

            if (randomValue <= cumulativeWeight)
            {
                selectedIndex = j;
                break;
            }
        }

        return selectedIndex;
    }
}