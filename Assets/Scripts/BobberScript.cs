using UnityEngine;

public class BobberScript : MonoBehaviour
{
    private Rigidbody bobberRb;
    private void OnCollisionEnter(Collision collision)
    {
        bobberRb = GetComponent<Rigidbody>();  

        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            bobberRb.velocity = Vector3.zero;
            bobberRb.useGravity = false;
        }

    }
}
