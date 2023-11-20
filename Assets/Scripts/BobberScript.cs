using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobberScript : MonoBehaviour
{
    [SerializeField] GameObject hook, gameController;
    [SerializeField] ParticleSystem splashParticles;

    private GameObject newHook;
    private Rigidbody bobberRb;
    private PlayerInput playerInputSwap;
    private PlayerController playerController;

    private float splashDestroyWaitTime = 3f;

    private void Start()
    {
        playerInputSwap = gameController.GetComponent<PlayerInput>();
        playerController = gameController.GetComponent<PlayerController>();
        bobberRb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            ParticleSystem newSplashParticles = Instantiate(splashParticles, transform.position, Quaternion.Euler(splashParticles.transform.eulerAngles));
            newSplashParticles.Play();
            Destroy(newSplashParticles.gameObject, splashDestroyWaitTime);

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
