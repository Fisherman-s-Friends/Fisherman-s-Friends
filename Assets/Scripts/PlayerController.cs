using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody bobberRb;
    [SerializeField] GameObject castBar, minigameObj, minigameSliderObj, catchArea;
    [SerializeField] BobberScript bobberScript;
    [SerializeField] float controlSpeed;

    public Rigidbody hookRb;

    private MinigameScript mgScript;
    private GameObject closestFish;
    private Transform hookTrans;
    private Collider hookCollider;
    private Slider castSlider, minigameSlider;
    private SessionController sessionController;

    private Vector3 bobberStartPos;
    private Vector2 movementDir;

    private float holdStarted, holdEnded, holdTotal;
    private float castLineX = 2, castLineY = 2, fishFromHook = 1f;
    private bool haveYouCasted = false, sliderBarCheck = false;
    private bool gameStarted = false, mainMenuPressed = false;

    void Start()
    {
        sessionController = GameObject.Find("SceneManager").GetComponent<SessionController>();

        bobberStartPos = bobberRb.transform.localPosition;
        castSlider = castBar.GetComponent<Slider>();
        minigameSlider = minigameSliderObj.GetComponent<Slider>();
        mgScript = minigameObj.GetComponent<MinigameScript>();
    }
    void Update()
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

            if (holdTotal >= 3.5f) { holdTotal = 3.5f; }
            bobberRb.velocity = new Vector3(castLineX * (holdTotal * 3), castLineY * (holdTotal), 0);
            haveYouCasted = true;
            sliderBarCheck = false;
            castBar.SetActive(false);
        }
    }
    private IEnumerator SliderChargeUp()
    {
        while (sliderBarCheck)
        {
            castSlider.value += 1 * Time.deltaTime;
            yield return null;
        }
    }

    public void StopHook()
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
        if (mainMenuPressed) return; mainMenuPressed = true;

        StartCoroutine(SceneController.ChangeScene(Scenes.Home));
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
        {
            closestFish.GetComponent<FishScript>().FishInMovement(false);
        }
        else if (closestFish != null)
        {
            sessionController.AddMoney(closestFish.GetComponent<FishScript>().fishValue.value);
            Destroy(closestFish);
        }

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

