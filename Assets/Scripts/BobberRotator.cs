using UnityEngine;

public class BobberRotator : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] Rigidbody bobberObj;

    private Quaternion targetRotation;
    private Vector3 castDirection;
    private Vector3 startPos;

    private float rotationSpeed = 0.5f;

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
        castDirection = (targetPos.transform.position - transform.position).normalized;

        if (!bobberObj.useGravity && startPos != transform.position)
        {
            targetRotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(castDirection, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
}
