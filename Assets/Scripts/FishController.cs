using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField]
    public Vector3 fishBoundingBoxOffset;

    [SerializeField]
    public Vector3 fishBoundingBoxSize;

    [SerializeField]
    private GameObject fishObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void SpawnFish()
    {
        var fish = Instantiate(fishObject, transform.position, transform.rotation);
        fish.GetComponent<FishScript>().Controller = this;
    }
}
