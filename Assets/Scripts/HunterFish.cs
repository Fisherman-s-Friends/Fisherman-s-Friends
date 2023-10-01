using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.PlayerSettings;

public class HunterFish : DetectingFish
{
    [SerializeField]
    private bool cannibal;

    [SerializeField]
    private float huntingSpeed;

    [SerializeField]
    private float attackThreshold;

    [SerializeField] 
    private float attackCooldown;

    private float attackTimer;

    private bool hunting;

    protected override void Start()
    {
        attackTimer = 0;

        base.Start();
    }

    protected override void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        base.Update();
    }

    protected override void Move()
    {
        if (!hunting)
        {
            if (closeColliders.Count == 0)
            {
                base.Move();
                return;
            }

            hunting = true;
        }

        if(closeColliders.Count == 0 || attackTimer > 0) 
        {
            hunting = false;
            base.Move();
            return;
        }

        var pos = transform.position;

        var closestPrey = closeColliders.Where(o => o != null).Select(o => o).OrderBy(o => (pos - o.transform.position).magnitude).FirstOrDefault();
        
        // The list contains only deleted fish
        if(closestPrey == null)
        {
            closeColliders.Clear();
            base.Move();
            return;
        }

        var closestPreyPos = closestPrey.transform.position;

        if(Vector3.Distance(closestPreyPos, pos) < attackThreshold)
        {
            closeColliders.Remove(closestPrey);
            controller.KillFish(closestPrey.gameObject);
            attackTimer = attackCooldown;
            target = controller.MovePointInsideBounds(transform.forward * maxTargetPointDistance, maxTargetPointDistance);
            return;
        }

        var newDir = Vector3.RotateTowards(transform.forward, closestPreyPos - pos, turningSpeed * Time.deltaTime, turningSpeedChange);
        transform.position += newDir * (huntingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
    }

    protected override bool FilterCollider(Collider other)
    {
        return controller.IsFish(other.gameObject) && (!CompareTag(other.tag) || cannibal);
    }
}
