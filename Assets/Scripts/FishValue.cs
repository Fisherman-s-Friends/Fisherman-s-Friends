using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FishValue : ScriptableObject
{
    public int value;
    public virtual int Value()
    {
        return value;
    }
}
