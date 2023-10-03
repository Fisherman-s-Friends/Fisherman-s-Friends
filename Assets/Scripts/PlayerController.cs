using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody bobberRb;
    [SerializeField] private GameObject castBar;
    [SerializeField] BobberScript bobberScript;

    private float castLineX = 2, castLineY = 2;
    private float holdStarted, holdEnded, holdTotal;
    private bool haveYouCasted = false, sliderBarCheck = false;
    private Slider castSlider;
    private Vector3 startPos;

    public void Start()
    {
        // Stores the bobbers position on start
        startPos = bobberRb.transform.localPosition;
        castSlider = castBar.GetComponent<Slider>();
    }

    // Triggered with Space-key, casts the bobber
    public void StartCast(InputAction.CallbackContext context)
    {

        // Check if the bobber has been already cast
        if (!haveYouCasted)
        {
            if (context.started) // .started triggers on key down
            {
                castBar.SetActive(true);
                holdStarted = Time.realtimeSinceStartup;
                sliderBarCheck = true;
                StartCoroutine(SliderChargeUp());
            }
            if (context.canceled) // .canceled triggers on key release
            {
                holdEnded = Time.realtimeSinceStartup;
                bobberRb.useGravity = true;
                holdTotal = holdEnded - holdStarted;

                if (holdTotal >= 3.5f) // Limits the total distance that can be cast, even though keydown lasts for ages 
                {
                    holdTotal = 3.5f;
                }
                bobberRb.velocity = new Vector3(castLineX * (holdTotal + holdTotal + holdTotal), castLineY * (holdTotal), 0);
                haveYouCasted = true;
                sliderBarCheck = false;
                castBar.SetActive(false);
                Debug.Log("Total time space was hold down: " + holdTotal); // Debug for the cast time multiplier
            }
        }
    }

    // Triggered with Z-key, resets the position of the bobber for a recast
    public void ResetCast(InputAction.CallbackContext context)
    {
        // checks if the bobber is in the air and moving with velocity
        if (bobberRb.velocity.x == 0 || bobberRb.velocity.y == 0)
        {
            bobberRb.useGravity = false;
            bobberRb.transform.localPosition = startPos;
            bobberRb.velocity = new Vector3(0, 0, 0);
            haveYouCasted = false;
            castSlider.value = 0;
            bobberScript.DestroyHook();
        }
        else
        {
            Debug.Log("Bobber in motion, cannot recast yet!");
        }
    }

    // Updates the values to the castbar slider
    private IEnumerator SliderChargeUp()
    {
        while (sliderBarCheck)
        {
            castSlider.value += 1 * Time.deltaTime;
            yield return null;
        }
    }
}
