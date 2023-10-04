using UnityEngine;
using UnityEngine.InputSystem;

public class BobberScript : MonoBehaviour
{
    [SerializeField]
    private GameObject hook, gameController;

    private GameObject newHook;
    private Rigidbody bobberRb;
    private PlayerInput playerInputSwap;


    private void Start()
    {
        playerInputSwap = gameController.GetComponent<PlayerInput>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        bobberRb = GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            bobberRb.velocity = Vector3.zero;
            bobberRb.useGravity = false;
            newHook = Instantiate(hook, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
            playerInputSwap.SwitchCurrentActionMap("HookActions");
        }
    }
    public void DestroyHookAndSwapActionMap()
    {

        playerInputSwap.SwitchCurrentActionMap("RodActions");
        Destroy(newHook);
    }
}
