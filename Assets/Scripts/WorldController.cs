using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    private int numberOfPlaces;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private EnvPrefabs[] envPrefabs;

    [SerializeField] 
    private Transform envTransform;

    [SerializeField]
    private float offsetY;

    void Start()
    {
        for (int i = 0; i < numberOfPlaces; i++)
        {
            Instantiate(RandomUtil.GetRandomElemntFromWeightedList(envPrefabs.Select(p => new System.Tuple<GameObject, float>(p.prefab, p.weight * 0.1f)).ToList()),
                transform.position - new Vector3(spacing * numberOfPlaces / 2 - spacing / 2, offsetY, 0) + new Vector3(i * spacing, 0, 0),
                Quaternion.identity,
                envTransform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < numberOfPlaces; i++)
        {
            Gizmos.DrawWireSphere(transform.position - new Vector3(spacing * numberOfPlaces / 2 - spacing / 2 , offsetY, 0) + new Vector3 (i * spacing, 0, 0), 2.5f);
        }
    }

    [System.Serializable]
    struct EnvPrefabs
    {
        public GameObject prefab;
        [Range(1, 10)]
        public int weight;
    }
}

public static class RandomUtil
{
    public static T GetRandomElemntFromWeightedList<T>( IList<System.Tuple <T, float>> pairs)
    {
        float maxWeight = pairs.Sum(p => p.Item2);
        float rnd = Random.Range(0, maxWeight);
        float runningWeight = pairs[0].Item2;
        for(int i = 0; i < pairs.Count(); i++)
        {
            var currentWeight = pairs[i].Item2;
            if (rnd < runningWeight)
                return pairs[i].Item1;
            runningWeight += currentWeight;
        }
        return pairs.Last().Item1;
    }
}
