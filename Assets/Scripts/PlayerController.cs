using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody bobberRb;
    [SerializeField] private GameObject castBar, minigameObj, minigameSliderObj, catchArea;
    [SerializeField] BobberScript bobberScript;

    public bool hookGetFish = false; // change this to getter/setter
    private float holdStarted, holdEnded, holdTotal, controlSpeed = 750, castLineX = 2, castLineY = 2, hookDist = 0.5f;
    private bool haveYouCasted = false, sliderBarCheck = false, gameStarted = false;
    private Slider castSlider, minigameSlider;
    public Rigidbody hookRb;
    private Vector3 bobberStartPos;
    private Vector2 movementDir;
    private MinigameScript mgScript;
    private GameObject closestFish;
    private Transform hookTrans;
    private SphereCollider hookCollider;

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

        if (hookTrans == null)
        {
            return;
        }

        if (gameStarted == true && Vector3.Distance(hookTrans.position, closestFish.transform.position) < hookDist)
        {
            mgScript.GetFishBehaviour(closestFish.GetComponent<FishScript>().fishBehaviour);
            minigameObj.SetActive(true);
            gameStarted = false;
        }
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
            hookRb.useGravity = false;
            hookRb.velocity = Vector3.zero;
            haveYouCasted = false;
            hookGetFish = true;
            hookCollider.enabled = true;
        }
    }

    public void MinigameInput(InputAction.CallbackContext context)
    {
        movementDir = context.ReadValue<Vector2>();
    }

    public void ResetEverything()
    {
        minigameObj.SetActive(false);
        bobberRb.useGravity = false;
        bobberRb.transform.localPosition = bobberStartPos;
        bobberRb.velocity = new Vector3(0, 0, 0);
        haveYouCasted = false;
        castSlider.value = 0;
        minigameSlider.value = 0;
        bobberScript.DestroyHookAndSwapActionMap();
        Destroy(closestFish);
        hookGetFish = false;
    }

    public void FishHookedStartGame(GameObject fish, Transform hooktransform)
    {
        hookTrans = hooktransform;
        closestFish = fish;
        gameStarted = true;
    }

    public void GetHookCollider(SphereCollider collider)
    {
            hookCollider = collider;
    }

}

