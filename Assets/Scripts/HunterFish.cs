using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HunterFish : FishScript
{

    [SerializeField]
    private float senseRadius;

    [SerializeField]
    private bool cannibal;

    [SerializeField]
    private float huntingSpeed;

    [SerializeField]
    private float attackThershold;

    [SerializeField] 
    private float attackCooldown;

    private float attackTimer;

    private List<Collider> closePrey;

    private bool hunting;

    private void Start()
    {
        attackTimer = 0;

        closePrey = new List<Collider>();
        var preyTriggerCollider = gameObject.AddComponent<SphereCollider>();
        preyTriggerCollider.radius = senseRadius;
        preyTriggerCollider.isTrigger = true;

        base.Start();
    }

    void Update()
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
            base.Move();
            return;
        }

        if(closePrey.Count == 0 || attackTimer > 0) 
        {
            hunting = false;
            return;
        }

        var closestPrey = closePrey.Where(o => o != null).Select(o => o).OrderBy(o => (transform.position - o.transform.position).magnitude).First();
        var closestPreyPos = closestPrey.transform.position;

        if(Vector3.Distance(closestPreyPos, transform.position) < attackThershold)
        {
            closePrey.Remove(closestPrey);
            controller.KillFish(closestPrey.gameObject);
            attackTimer = attackCooldown;
            return;
        }

        var newDir = Vector3.RotateTowards(transform.forward, closestPreyPos - transform.position, turningSpeed * Time.deltaTime, turningSpeedChange);
        transform.position += newDir * (huntingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && controller.isFish(other.gameObject))
        {
            if(other.tag == tag && !cannibal)
            {
                return;
            }

            closePrey.Add(other);
            hunting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && (other.tag != tag || cannibal))
        {
            if (closePrey.Contains(other))
                closePrey.Remove(other);
        }
    }
}
