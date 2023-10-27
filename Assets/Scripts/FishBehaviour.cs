using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class FishBehaviour : ScriptableObject
{
    // Lower SPEED value = faster fish movement -> fish moves more often inside a time period
    // Lower SMOOTHNESS value = more "snappy" movement -> the time it takes to move from A to B

    // DEFAULTS (for reference):
    // Quick fish:      speed 20, smoothness 5
    // Default fish:    speed 30, smoothness 15
    // Lazy fish:       speed 60, smoothness 25

    public float speed, smoothness;

    public virtual float FishMoveSpeed()
    {
        return speed;
    }

    public virtual float FishMoveSmoothness()
    {
        return smoothness;
    }
}
