using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWinDespawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
