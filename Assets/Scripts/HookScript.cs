using UnityEngine;

public class HookScript : MonoBehaviour
{
    [SerializeField] float stopHookDistance = 0.5f;

    private PlayerController playerController;
    private GameObject closestFish;
    private FishScript fishScript;
    private Transform hookTrans;
    private Collider hookCollider;

    private string[] ignoredTags = { "boidFish", "Fish", "WaterPlane" };
    private bool fishOnHook = false;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>();
        hookCollider = GetComponent<Collider>();
        hookCollider.enabled = false;
        playerController.GetHookCollider(hookCollider);
    }

    private void Update()
    {
        HookEnvironmentRaycast();

        if (fishOnHook)
        {
            playerController.FishHookedStartGame(closestFish, hookTrans);
            fishOnHook = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (closestFish != null) { return; }

        if (collision.gameObject.tag == "Fish" || collision.gameObject.tag == "boidFish")
        {
            closestFish = collision.gameObject;
            fishScript = closestFish.GetComponent<FishScript>();
            hookTrans = transform;
        }

        if (fishScript != null)
        {
            fishScript.GetHook(hookTrans.position);
            fishOnHook = true;
        }
    }

    private void HookEnvironmentRaycast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (!RaycastDown(ray, out hit)) return;

        string objTag = hit.collider.gameObject.tag;
        if (System.Array.Exists(ignoredTags, tag => tag == objTag)) return;

        if (hit.distance <= stopHookDistance)
        {
            playerController.StopHook();
        }
    }

    private bool RaycastDown(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit);
    }
}
