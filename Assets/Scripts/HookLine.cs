using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLine : LineController
{
    private Transform[] startAndEndPosition = new Transform[2];
    [SerializeField] Transform hookPos;
    void Start()
    {
        Transform bobberPos = GameObject.Find("Bobber").GetComponent<Transform>();
        startAndEndPosition[1] = bobberPos;
        startAndEndPosition[0] = hookPos;

        SetUpLine(startAndEndPosition);
    }
}
