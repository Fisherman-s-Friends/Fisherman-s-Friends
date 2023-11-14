using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;

    [SerializeField]
    private float lineOffset;

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
            if (i == points.Length - 1)
            {
                Vector3 offsetPosition = new Vector3(points[i].position.x, points[i].position.y + lineOffset, points[i].position.z);
                lr.SetPosition(i, offsetPosition);
            } else
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}
