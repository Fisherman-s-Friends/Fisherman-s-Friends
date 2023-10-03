using UnityEngine;

public class BobberScript : MonoBehaviour
{
    [SerializeField]
    private GameObject hook;

    private GameObject newHook;
    private Rigidbody bobberRb;

    public void OnCollisionEnter(Collision collision)
    {
        bobberRb = GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            bobberRb.velocity = Vector3.zero;
            bobberRb.useGravity = false;
            newHook = Instantiate(hook, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
        }
    }
    public void DestroyHook()
    {
        Destroy(newHook);
    }
}
