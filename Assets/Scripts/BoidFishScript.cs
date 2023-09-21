using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class BoidFishScript : FishScript
{
    [SerializeField]
    private float boidSenseRadius;

    [SerializeField]
    private float randomDirectionWeight;

    private List<Collider> closeBoids;

    private void Start()
    {
        closeBoids = new List<Collider>();
        var boidTriggerCollider = gameObject.AddComponent<SphereCollider>();
        boidTriggerCollider.radius = boidSenseRadius;
        boidTriggerCollider.isTrigger = true;
    }

    protected override void CreateNewTarget()
    {
        var scripts = closeBoids.Where(c => c != null).Select(c => c.GetComponent<BoidFishScript>());

        Vector3 avarageDirection = Vector3.zero;

        foreach (var script in scripts)
        {
            if(script)
                avarageDirection += script.GetDirection();
        }

        avarageDirection = avarageDirection.normalized;

        target = transform.position + ((Random.rotation * Vector3.forward * randomDirectionWeight) + avarageDirection) * Random.Range(minTargetPointDistance, maxTargetPointDistance);

        KeepTargetInsideBounds();
    }

    public Vector3 GetDirection()
    {
        return (target - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag==tag)
        {
            closeBoids.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.tag == tag)
        {
            if (closeBoids.Contains(other))
                closeBoids.Remove(other);
        }
    }
}
