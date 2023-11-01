using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookScript : MonoBehaviour
{
    private GameObject closestFish;

    private void OnTriggerEnter(Collider collision)
    {
        if (closestFish != null) { return; }
        if (collision.gameObject.tag == "Fish" || collision.gameObject.tag == "boidFish")
        {
            closestFish = collision.gameObject;
            closestFish.transform.localScale = new Vector3(5, 5, 5);
            Debug.Log("A fish is near!");
        }
    }

}
