using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField]
    private Vector3 fishBoundingBoxOffset;

    [SerializeField]
    private Vector3 fishBoundingBoxSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + fishBoundingBoxOffset, fishBoundingBoxSize);
    }
}
