using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private FishController controller;

    public FishController Controller { set { controller = value; } }

    [SerializeField]
    private float speed;

    [SerializeField] 
    private float thershold;

    [SerializeField]
    private float minTargetPointDistance;

    [SerializeField]
    private float maxTargetPointDistance;

    [SerializeField] private float turningSpeed;
    [SerializeField] private float turningSpeedChange;

    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // save position as variable to make code efficient see: https://github.com/JetBrains/resharper-unity/wiki/Avoid-multiple-unnecessary-property-accesses
        var pos = transform.position;

        if(Vector3.Distance(pos, target) < thershold)
        {
            CreateNewTarget();
        }
        
        var newDir = Vector3.RotateTowards(transform.forward, target - pos, turningSpeed * Time.deltaTime, turningSpeedChange);
        transform.position += newDir * (speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
    }

    private void CreateNewTarget ()
    {
        target = transform.position + UnityEngine.Random.rotation * Vector3.forward * UnityEngine.Random.Range(minTargetPointDistance, maxTargetPointDistance);

        if (target.x < controller.fishBoundingBoxOffset.x - controller.fishBoundingBoxSize.x / 2 ||
            target.x > controller.fishBoundingBoxOffset.x + controller.fishBoundingBoxSize.x / 2) { 
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target, 0.2f);
        Gizmos.DrawLine(transform.position, target);
    }
}
