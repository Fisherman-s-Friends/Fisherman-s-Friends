using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControllerScript : MonoBehaviour
{
    [SerializeField] private Transform[] startAndEndPosition;
    private LineRenderer lr;
    private Transform[] points;


    // Start is called before the first frame update
    void Start()
    {
        SetUpLine(startAndEndPosition);
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }

    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }

    }
}
