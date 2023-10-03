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
        startAndEndPosition[0] = bobberPos;
        startAndEndPosition[1] = hookPos;

        SetUpLine(startAndEndPosition);
    }
}
