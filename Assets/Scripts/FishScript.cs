using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

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

    protected float collissionAvoidanceTurnRate = 10f;

    protected Vector3 target;

    public FishBehaviour fishBehaviour = null;

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
        point = controller.MovePointInsideBounds(point, maxTargetPointDistance);
        return AvoidCollissionsWithEnv(point);
    }

    protected Vector3 AvoidCollissionsWithEnv(Vector3 point)
    {
        var distanceModifier = 1f;
        // 1 << 3 -> Check for collissions only on layermask no. 3
        while (Raycast(point, 1 << 3))
        {
            point = RotatePointAroundFish(point, collissionAvoidanceTurnRate, distanceModifier);
            distanceModifier *= 0.9f;
        }
        return point;
    }

    /// <summary>
    /// Check if raycast from fish to a point hits anything
    /// </summary>
    /// <param name="point">Target point</param>
    /// <param name="layerMask">Mask</param>
    /// <returns></returns>
    protected bool Raycast(Vector3 point, int layerMask)
    {
        var pos = transform.position;
        var dir = point - pos;
        return Physics.Raycast(pos, dir, dir.magnitude, layerMask);
    }

    private Vector3 RotatePointAroundFish(Vector3 point, float amount, float distanceModifier)
    {
        var pos = transform.position;
        return pos + Quaternion.AngleAxis(amount, Vector3.up) * (point - pos) * distanceModifier;
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

    public void GetHook()
    {

    }
}
