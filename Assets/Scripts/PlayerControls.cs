using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody bobberObj;
    [SerializeField]
    private float castLineX, castLineY;

    private float holdStarted, holdEnded, holdTotal;

    public void StartCast(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            holdStarted = Time.realtimeSinceStartup;
        }
        if (context.canceled)
        {
            holdEnded = Time.realtimeSinceStartup;
            bobberObj.useGravity = true;
            holdTotal = holdEnded - holdStarted;
            if (holdTotal > 3f)
            {
                holdTotal = 3f;
            }
            bobberObj.velocity = new Vector3(castLineX * (holdTotal+holdTotal+holdTotal), castLineY*(holdTotal), 0);

            Debug.Log("Total time space was hold down: " + holdTotal);
        }
    }
}
