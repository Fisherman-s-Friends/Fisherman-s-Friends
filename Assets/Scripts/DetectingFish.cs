using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingFish : FishScript
{
    [SerializeField] private float detectionRange;

    protected List<Collider> closeColliders;

    protected override void Start()
    {

        closeColliders = new List<Collider>();
        var triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.radius = detectionRange;
        triggerCollider.isTrigger = true;

        base.Start();
    }

    /// <summary>
    /// Filter collider when it enters the detection range
    /// </summary>
    /// <param name="other">The collider</param>
    /// <returns>If the collider should be counted</returns>
    protected virtual bool FilterCollider(Collider other)
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && FilterCollider(other))
        {
            closeColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && closeColliders.Contains(other))
        {
            closeColliders.Remove(other);
        }
    }
}
