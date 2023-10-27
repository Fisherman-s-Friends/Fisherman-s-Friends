using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class FishBehaviour : ScriptableObject
{
    public float speed;        
    private float boundaries = 333f, newPosition, direction;

    public virtual float FishMovement(Vector3 currentPosition)
    {

        newPosition = speed * -direction + currentPosition.x; // Maths for plub

        if (newPosition > boundaries)
        {
            newPosition = boundaries;
            direction = -direction;
        }
        else if (newPosition < -boundaries)
        {
            newPosition = -boundaries;
            direction = -direction;
        }

        return newPosition;
    }
}
