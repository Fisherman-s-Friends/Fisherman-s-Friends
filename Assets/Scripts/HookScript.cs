using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private Rigidbody hook;
    public float hookSpeed = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        hook = GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("WaterPlane"))
        {
            hook.useGravity = false;
            Vector3 upwardForce = Vector3.up * hookSpeed;
            hook.AddForce(upwardForce, ForceMode.Impulse);

        }

    }
}
