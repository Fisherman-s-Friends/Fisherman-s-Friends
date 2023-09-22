using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    protected FishController controller;

    public FishController Controller { set { controller = value; } }

    [SerializeField]
    protected float speed;

    [SerializeField] 
    private float thershold;

    [SerializeField]
    protected float minTargetPointDistance;

    [SerializeField]
    protected float maxTargetPointDistance;

    [SerializeField] protected float turningSpeed;
    [SerializeField] protected float turningSpeedChange;

    [SerializeField] private GameObject deathParticle;

    protected Vector3 target;

    // Start is called before the first frame update
    protected void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        // save position as variable to make code efficient see: https://github.com/JetBrains/resharper-unity/wiki/Avoid-multiple-unnecessary-property-accesses
        var pos = transform.position;

        if (Vector3.Distance(pos, target) < thershold)
        {
            CreateNewTarget();
        }

        var newDir = Vector3.RotateTowards(transform.forward, target - pos, turningSpeed * Time.deltaTime, turningSpeedChange);
        transform.position += newDir * (speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
    }

    protected virtual void CreateNewTarget ()
    {
        target = transform.position + UnityEngine.Random.rotation * Vector3.forward * UnityEngine.Random.Range(minTargetPointDistance, maxTargetPointDistance);
        KeepTargetInsideBounds();
    }

    protected void KeepTargetInsideBounds()
    {
        if (target.x < controller.fishBoundingBoxOffset.x - controller.fishBoundingBoxSize.x / 2 ||
            target.x > controller.fishBoundingBoxOffset.x + controller.fishBoundingBoxSize.x / 2)
        {
            target.x += target.x < controller.fishBoundingBoxOffset.x ? maxTargetPointDistance : -maxTargetPointDistance;
        }

        if (target.z < controller.fishBoundingBoxOffset.z - controller.fishBoundingBoxSize.z / 2 ||
            target.z > controller.fishBoundingBoxOffset.z + controller.fishBoundingBoxSize.z / 2)
        {
            target.z += target.z < controller.fishBoundingBoxOffset.z ? maxTargetPointDistance : -maxTargetPointDistance;
        }

        if (target.y < controller.fishBoundingBoxOffset.y - controller.fishBoundingBoxSize.y / 2 ||
            target.y > controller.fishBoundingBoxOffset.y + controller.fishBoundingBoxSize.y / 2)
        {
            target.y += target.y < controller.fishBoundingBoxOffset.y ? maxTargetPointDistance : -maxTargetPointDistance;
        }
    }

    public void Kill()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target, 0.2f);
        Gizmos.DrawLine(transform.position, target);
    }
}
