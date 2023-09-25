using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FishController : MonoBehaviour
{
    [SerializeField]
    public Vector3 fishBoundingBoxOffset;

    [SerializeField]
    public Vector3 fishBoundingBoxSize;

    [SerializeField]
    private GameObject fishObject;

    private List<GameObject> fishList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Kills a given fish
    /// </summary>
    /// <param name="fish">Fish to kill</param>
    public void KillFish(GameObject fish)
    {
        fishList.Remove(fish);
        fish.GetComponent<FishScript>().Kill();
    }

    /// <summary>
    /// Check if given game object is a fish in <c>fishList</c>
    /// </summary>
    /// <param name="gameObject">Object to check</param>
    /// <returns>Is object a fish or not</returns>
    public bool IsFish(GameObject gameObject)
    {
        return fishList.Contains(gameObject);
    }

    /// <summary>
    /// If the given point is outside of the bounds. Move it inside the bounds
    /// </summary>
    /// <param name="point">The point to move</param>
    /// <param name="moveAmount">Amount that is possible to move the point by</param>
    /// <returns>Point inside bounds</returns>
    public Vector3 MovePointInsideBounds(Vector3 point, float moveAmount)
    {
        if (point.x < fishBoundingBoxOffset.x - fishBoundingBoxSize.x / 2 ||
            point.x > fishBoundingBoxOffset.x + fishBoundingBoxSize.x / 2)
        {
            point.x += point.x < fishBoundingBoxOffset.x ? moveAmount : -moveAmount;
        }

        if (point.z < fishBoundingBoxOffset.z - fishBoundingBoxSize.z / 2 ||
            point.z > fishBoundingBoxOffset.z + fishBoundingBoxSize.z / 2)
        {
            point.z += point.z < fishBoundingBoxOffset.z ? moveAmount : -moveAmount;
        }

        if (point.y < fishBoundingBoxOffset.y - fishBoundingBoxSize.y / 2 ||
            point.y > fishBoundingBoxOffset.y + fishBoundingBoxSize.y / 2)
        {
            point.y += point.y < fishBoundingBoxOffset.y ? moveAmount : -moveAmount;
        }
        return point;
    }

    /// <summary>
    /// Spawn a fish
    /// </summary>
    private void SpawnFish()
    {
        var fish = Instantiate(fishObject, transform.position, transform.rotation);
        fishList.Add(fish);
        fish.GetComponent<FishScript>().Controller = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + fishBoundingBoxOffset, fishBoundingBoxSize);
    }

    private void OnGUI()
    {
        // Temporary until a spawner is created
        if (GUI.Button(new Rect(10, 70, 100, 30), "Spawn a fish"))
            SpawnFish();
    }
}
