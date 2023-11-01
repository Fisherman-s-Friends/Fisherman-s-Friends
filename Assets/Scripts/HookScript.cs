using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookScript : MonoBehaviour
{
    private GameObject closestFish;

    private FishScript fishScript;

    private void Start()
    {
        fishScript = fishScript.GetComponent<FishScript>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (closestFish != null) { return; }
        if (collision.gameObject.tag == "Fish" || collision.gameObject.tag == "boidFish")
        {
            closestFish = collision.gameObject;
            fishScript.GetHook();

            Debug.Log("A fish is near!");
        }

    }
}
