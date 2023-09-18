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

    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target) < thershold)
        {
            CreateNewTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target, 0.25f);
    }
}
