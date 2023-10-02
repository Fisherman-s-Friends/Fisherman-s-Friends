using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class BoidFishScript : DetectingFish
{
    [SerializeField]
    private float randomDirectionWeight;
    

    /// <summary>
    /// Get the direction that this fish is moving in.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirection()
    {
        return (target - transform.position).normalized;
    }

    /// <inheritdoc cref="FishScript.CreateNewTarget()"/>
    protected override Vector3 CreateNewTarget()
    {
        var scripts = closeColliders.Where(c => c != null).Select(c => c.GetComponent<BoidFishScript>());

        Vector3 avarageDirection = Vector3.zero;

        foreach (var script in scripts)
        {
            if(script)
                avarageDirection += script.GetDirection();
        }

        avarageDirection = avarageDirection.normalized;

        var point= transform.position + ((Random.rotation * Vector3.forward * randomDirectionWeight) + avarageDirection) * Random.Range(minTargetPointDistance, maxTargetPointDistance);
        point = controller.MovePointInsideBounds(point, maxTargetPointDistance);
        return AvoidCollissionsWithEnv(point);
    }

    protected override bool FilterCollider(Collider other)
    {
        return CompareTag(other.tag);
    }
}
