using UnityEngine;
using UnityEngine.InputSystem;

public class BobberScript : MonoBehaviour
{
    [SerializeField] GameObject hook, gameController;

    private GameObject newHook;
    private Rigidbody bobberRb;
    private PlayerInput playerInputSwap;
    private PlayerController playerController;

    private void Start()
    {
        playerInputSwap = gameController.GetComponent<PlayerInput>();
        playerController = gameController.GetComponent<PlayerController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        bobberRb = GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            bobberRb.velocity = Vector3.zero;
            bobberRb.useGravity = false;
            newHook = Instantiate(hook, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
            playerController.hookRb = newHook.GetComponent<Rigidbody>();
            playerInputSwap.SwitchCurrentActionMap("HookActions");
        }
    }
    public void DestroyHookAndSwapActionMap()
    {
        playerInputSwap.SwitchCurrentActionMap("RodActions");
        Destroy(newHook);
    }
}
