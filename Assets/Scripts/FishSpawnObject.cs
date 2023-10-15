using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FishSpawnObject")]
public class FishSpawnObject : ScriptableObject
{
    public GameObject prefab;
    [Tooltip("Amount of fish to spawn")]
    public int amount;
    [Range(0, 10), Tooltip("Weight of this type when it is selected randomly")]
    public int weight;
    [Tooltip("Limit the fish to only one on the screen at a time")]
    public bool Limit;
}
