using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody bobberObj;

    private float holdStarted, holdEnded, holdTotal;

    private void Start()
    {

    }


    public void StartCast(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            holdStarted = Time.realtimeSinceStartup;
        }
        if (context.canceled)
        {
            holdEnded = Time.realtimeSinceStartup;
            //bobberObj.useGravity = true;
            holdTotal = holdEnded - holdStarted;

            Debug.Log("Total time space was hold down: " + holdTotal);
        }
    }
}
