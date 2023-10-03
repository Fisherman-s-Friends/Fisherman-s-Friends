using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberLine : LineController
{
    [SerializeField]
    private Transform[] startAndEndPosition;

    void Start()
    {
        SetUpLine(startAndEndPosition);
    }
}
