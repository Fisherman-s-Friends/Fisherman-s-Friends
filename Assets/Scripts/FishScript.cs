using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FishScript : MonoBehaviour
{
    protected FishController controller;

    public FishController Controller { set { controller = value; } }

    [SerializeField]
    protected float speed;

    [SerializeField] 
    private float threshold;

    [SerializeField]
    protected float minTargetPointDistance;

    [SerializeField]
    protected float maxTargetPointDistance;

    [SerializeField] protected float turningSpeed;
    [SerializeField] protected float turningSpeedChange;

    [SerializeField] private GameObject deathParticle;

    protected Vector3 target;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    /// <summary>
    /// Moves the fish towards its target
    /// </summary>
    protected virtual void Move()
    {
        // save position as variable to make code efficient see: https://github.com/JetBrains/resharper-unity/wiki/Avoid-multiple-unnecessary-property-accesses
        var pos = transform.position;

        if (Vector3.Distance(pos, target) < threshold)
        {
            target = CreateNewTarget();
        }

        var newDir = Vector3.RotateTowards(transform.forward, target - pos, turningSpeed * Time.deltaTime, turningSpeedChange);
        transform.position += newDir * (speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
    }

    /// <summary>
    /// Create a new target position
    /// </summary>
    protected virtual Vector3 CreateNewTarget()
    {
        var point = transform.position + UnityEngine.Random.rotation * Vector3.forward * UnityEngine.Random.Range(minTargetPointDistance, maxTargetPointDistance);
        return controller.MovePointInsideBounds(point, maxTargetPointDistance);
    }

    /// <summary>
    /// Destroy the gameobject and instantiate death particle effect in its place.
    /// </summary>
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
