using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookScript : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject closestFish;
    private FishScript fishScript;
    private bool fishOnHook = false;
    private Transform hookTrans;
    private SphereCollider hookCollider;


    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>();
        hookCollider = GetComponent<SphereCollider>();
        hookCollider.enabled = false;
        playerController.GetHookCollider(hookCollider);
    }
    private void Update()
    {
        if (fishOnHook)
        {
            playerController.FishHookedStartGame(closestFish, hookTrans);
            fishOnHook = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (closestFish != null) { return; }

        if (collision.gameObject.tag == "Fish" || collision.gameObject.tag == "boidFish")
        {
            closestFish = collision.gameObject;
            fishScript = closestFish.GetComponent<FishScript>();
            hookTrans = transform;
        }

        if (fishScript != null)
        {
            fishScript.GetHook(hookTrans.position);
            fishOnHook = true;
        }
    }

}
