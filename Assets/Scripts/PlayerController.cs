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


    private float castLineX = 2, castLineY = 2;
    private float holdStarted, holdEnded, holdTotal, minigameCtrlSpeed = 8f;
    //added bool to check if hook has been stopped
    private bool haveYouCasted = false, sliderBarCheck = false, hookStopped = false, arrowKeysPressed = false;
    private Slider castSlider, minigameSlider;
    public Rigidbody hookRb;
    private Vector3 startPos, areaStartPos;
    private Vector2 movementDir;

    public void Start()
    {
        startPos = bobberRb.transform.localPosition;
        castSlider = castBar.GetComponent<Slider>();
        minigameSlider = minigameSliderObj.GetComponent<Slider>();
        areaStartPos = catchArea.transform.localPosition;
        //hookRb = hook.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (arrowKeysPressed)
        {
            if (catchArea.transform.localPosition.x >= 340 && movementDir.x == 1) { return; }
            else if (catchArea.transform.localPosition.x <= -340 && movementDir.x == -1) { return; }
            catchArea.transform.localPosition = new Vector2(catchArea.transform.localPosition.x + movementDir.x * minigameCtrlSpeed, areaStartPos.y);
        }
    }
    // Triggered with Space-key, casts the bobber
    public void StartCast(InputAction.CallbackContext context)
    {
        if (haveYouCasted) { return; }
        // Check if the bobber has been already cast
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
            bobberRb.velocity = new Vector3(castLineX * (holdTotal*3), castLineY * (holdTotal), 0);
            haveYouCasted = true;
            sliderBarCheck = false;
            castBar.SetActive(false);
        }
    }

    // Triggered with Z-key, resets the position of the bobber for a recast
    public void ResetCast(InputAction.CallbackContext context)
    {
        if (bobberRb.velocity.x == 0 || bobberRb.velocity.y == 0)
        {
            ResetEverything();
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

    public void MinigameMovement(InputAction.CallbackContext context)
    {
        arrowKeysPressed = false;
        if (minigameSlider.value == 100 && context.performed)
        {
            // here you could add the splash screen for the fish you caught
            ResetEverything();
        }
        movementDir = context.ReadValue<Vector2>();
        // Implement here the code for the checking & adding if the icon is inside the area
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            arrowKeysPressed = true;
            return;
        }
        minigameCtrlSpeed = 20f;
        if (catchArea.transform.localPosition.x >= 340 && movementDir.x == 1) { return; }
        else if (catchArea.transform.localPosition.x <= -340 && movementDir.x == -1) { return; }
        catchArea.transform.localPosition = new Vector2(catchArea.transform.localPosition.x + movementDir.x * (minigameCtrlSpeed), areaStartPos.y);
    }

    public void ResetEverything()
    {
        bobberRb.useGravity = false;
        bobberRb.transform.localPosition = startPos;
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
