using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody bobberObj;

    private float castLineX = 2, castLineY = 2;
    private float holdStarted, holdEnded, holdTotal;
    private bool haveYouCasted = false;
    private Vector3 startPos;

    public void Start()
    {
        // Stores the bobbers position on start
        startPos = bobberObj.transform.localPosition;
    }

    // Triggered with Space-key, casts the bobber
    public void StartCast(InputAction.CallbackContext context)
    {
        // Check if the bobber has been already cast
        if (!haveYouCasted)
        {
            if (context.started) // .started triggers on key down
            {
                holdStarted = Time.realtimeSinceStartup;
            }
            if (context.canceled) // .canceled triggers on key release
            {
                holdEnded = Time.realtimeSinceStartup;
                bobberObj.useGravity = true;
                holdTotal = holdEnded - holdStarted;

                if (holdTotal > 3f) // Limits the total distance that can be cast, even though keydown lasts for ages 
                {
                    holdTotal = 3f;
                }
                bobberObj.velocity = new Vector3(castLineX * (holdTotal + holdTotal + holdTotal), castLineY * (holdTotal), 0);
                haveYouCasted = true;
                Debug.Log("Total time space was hold down: " + holdTotal); // Debug for the cast time multiplier
            }
        }
    }

    // Triggered with Z-key, resets the position of the bobber for a recast
    public void ResetCast(InputAction.CallbackContext context)
    {
        // checks if the bobber is in the air and moving with velocity
        if (bobberObj.velocity.x == 0 || bobberObj.velocity.y == 0)
        {
            bobberObj.useGravity = false;
            bobberObj.transform.localPosition = startPos;
            bobberObj.velocity = new Vector3(0, 0, 0);
            haveYouCasted = false;
        }
        else
        {
            Debug.Log("Bobber in motion, cannot recast yet!");
        }
    }
}
