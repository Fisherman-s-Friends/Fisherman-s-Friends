using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class FishBehaviour : ScriptableObject
{
    // Lower SPEED value = faster fish movement -> fish moves more often inside a time period
    // Lower SMOOTHNESS value = more "snappy" movement -> the time it takes to move from A to B
    // Lower BARSIZE value = fish gets caught faster

    // DEFAULTS (for reference):
    // Quick fish:      speed 10, smoothness 5, barsize 100
    // Default fish:    speed 15, smoothness 5, barSize 150
    // Lazy fish:       speed 20, smoothness 10, barSize 220

    public float speed, smoothness, barSize;

    public virtual float FishMoveSpeed()
    {
        return speed;
    }

    public virtual float FishMoveSmoothness()
    {
        return smoothness;
    }

    public virtual float FishBarValueSize()
    {
        return barSize;
    }
}
