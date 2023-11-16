using UnityEngine;

public class HookLine : LineController
{
    [SerializeField] Transform hookPos;
    private Transform[] startAndEndPosition = new Transform[2];

    void Start()
    {
        Transform bobberPos = GameObject.Find("Bobber").GetComponent<Transform>();
        startAndEndPosition[1] = bobberPos;
        startAndEndPosition[0] = hookPos;

        SetUpLine(startAndEndPosition);
    }
}
