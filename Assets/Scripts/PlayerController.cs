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
    [SerializeField] private BobberScript bobberScript;

    public Rigidbody hookRb;

    private MinigameScript mgScript;
    private GameObject closestFish;
    private Transform hookTrans;
    private Collider hookCollider;
    private Slider castSlider, minigameSlider;

    private Vector3 bobberStartPos;
    private Vector2 movementDir;

    private float
        holdStarted, holdEnded, holdTotal,
        controlSpeed = 750, castLineX = 2, castLineY = 2, fishFromHook = 1f;
    private bool
        haveYouCasted = false, sliderBarCheck = false, gameStarted = false;

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

        if (gameStarted == true && Vector3.Distance(hookTrans.position, closestFish.transform.position) < fishFromHook)
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
            hookCollider.enabled = true;
        }
    }

    public void MainMenuButton()
    {
        SceneController.ChangeScene(Scenes.Home);
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
        bobberScript.DestroyHookAndSwapActionMap();
        haveYouCasted = false;
        castSlider.value = 0;
        if (closestFish != null && minigameSlider.value < minigameSlider.maxValue)
            closestFish.GetComponent<FishScript>().FishInMovement(false);
        else if (closestFish != null)
            Destroy(closestFish);
        minigameSlider.value = 0;
    }

    public void FishHookedStartGame(GameObject fish, Transform hooktransform)
    {
        hookTrans = hooktransform;
        closestFish = fish;
        gameStarted = true;
    }

    public void GetHookCollider(Collider collider)
    {
        hookCollider = collider;
    }

}

