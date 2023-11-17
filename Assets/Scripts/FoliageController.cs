using UnityEngine;

public class FoliageController : MonoBehaviour
{
    [SerializeField] GameObject[] foliagePrefabs;
    [SerializeField] int foliageCount;

    private Terrain terrain;
    private TerrainController terrainEditor;

    private string instantiatedTag = "Foliage";

    private float areaWidth = 360f;
    private float areaLength = 200f;
    private float areaOffset = 80f;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();

        if (terrainEditor != null)
        {
            terrainEditor.GenerateTerrain();
        }

        terrainEditor = GetComponent<TerrainController>();

        GenerateFoliage();
    }

    void GenerateFoliage()
    {
        if (terrain == null || foliagePrefabs == null || foliagePrefabs.Length == 0) { return; }

        Vector3 terrainCenter =
            new Vector3(terrain.transform.position.x, terrain.transform.position.y, terrain.transform.position.z);

        for (int i = 0; i < foliageCount; i++)
        {
            Vector3 randomPosition =
                new Vector3(Random.Range(areaOffset, areaWidth - areaOffset), 0f, Random.Range(-areaLength, areaLength));

            Vector3 foliagePosition = terrainCenter + randomPosition;

            RaycastHit hit;
            Ray ray = new Ray(new Vector3(foliagePosition.x, terrain.terrainData.size.y, foliagePosition.z), Vector3.down);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GroundPlane")))
            {
                foliagePosition.y = hit.point.y;
                GameObject newFoliage = Instantiate(foliagePrefabs[Random.Range(0, foliagePrefabs.Length)], foliagePosition, Quaternion.identity);
                newFoliage.tag = instantiatedTag;
            }
        }
    }
}
