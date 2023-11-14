using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobberRotator : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos;

    [SerializeField]
    private Rigidbody bobberObj;

    private Quaternion targetRotation;
    private Vector3 castDirection;
    private float rotationSpeed = 0.5f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    void FixedUpdate()
    {
        RotateBobber();
    }

    void RotateBobber()
    {
        if (!bobberObj.useGravity && startPos != transform.position)
        {
            castDirection = (targetPos.transform.position - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } else
        {
            castDirection = (targetPos.transform.position - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(castDirection, Vector3.up);
            transform.rotation = targetRotation;
            Debug.Log("Else");
        }
    }
}
