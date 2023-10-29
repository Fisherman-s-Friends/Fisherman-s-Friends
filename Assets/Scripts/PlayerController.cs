using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody bobberRb;
    [SerializeField] private GameObject castBar, minigameObj, minigameSliderObj, catchArea;
    //public GameObject hook;
    [SerializeField] BobberScript bobberScript;

    private float holdStarted, holdEnded, holdTotal, controlSpeed = 750, castLineX = 2, castLineY = 2;
    //added bool to check if hook has been stopped
    private bool haveYouCasted = false, sliderBarCheck = false, hookStopped = false;
    private Slider castSlider, minigameSlider;
    public Rigidbody hookRb;
    private Vector3 bobberStartPos;
    private Vector2 movementDir;
    private MinigameScript mgScript;

    public void Start()
    {
        bobberStartPos = bobberRb.transform.localPosition;
        castSlider = castBar.GetComponent<Slider>();
        minigameSlider = minigameSliderObj.GetComponent<Slider>();
        mgScript = minigameObj.GetComponent<MinigameScript>();
    }

    public void Update()
    {
        mgScript.MinigameMovement(movementDir, controlSpeed);
    }
    // Triggered with Space-key, casts the bobber
    public void StartCast(InputAction.CallbackContext context)
    {
        if (haveYouCasted) { return; }
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

            if (holdTotal >= 3.5f) { holdTotal = 3.5f; }    // Limits the total distance that can be cast, even though keydown lasts for ages 
            bobberRb.velocity = new Vector3(castLineX * (holdTotal * 3), castLineY * (holdTotal), 0);
            haveYouCasted = true;
            sliderBarCheck = false;
            castBar.SetActive(false);
        }
    }

    // Triggered with Z-key, resets the position of the bobber for a recast
    public void ResetCast(InputAction.CallbackContext context)
    {
        if (bobberRb.velocity.x == 0 || bobberRb.velocity.y == 0) { return; }
        ResetEverything();
    }

    private IEnumerator SliderChargeUp()
    {
        while (sliderBarCheck)
        {
            castSlider.value += 1 * Time.deltaTime;
            yield return null;
        }
    }
    //space bar or mouse 1 stops the hook in place
    public void StopHook(InputAction.CallbackContext context)
    {
        if (haveYouCasted)
        {
            //stops hook in place
            hookRb.useGravity = false;
            hookRb.velocity = Vector3.zero;
            hookStopped = true;

            minigameObj.SetActive(true);
            haveYouCasted = false;
        }
    }

    public void MinigameInput(InputAction.CallbackContext context)
    {
        movementDir = context.ReadValue<Vector2>();
    }

    public void ResetEverything()
    {
        bobberRb.useGravity = false;
        bobberRb.transform.localPosition = bobberStartPos;
        bobberRb.velocity = new Vector3(0, 0, 0);
        haveYouCasted = false;
        castSlider.value = 0;
        minigameSlider.value = 0;

        bobberScript.DestroyHookAndSwapActionMap();
        minigameObj.SetActive(false);
    }

    public void MoveHook(InputAction.CallbackContext context)
    {
        if (hookStopped)
        {
            //moving the hook up and down

        }
    }


}
