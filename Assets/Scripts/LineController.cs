using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] float lineOffset;

    private LineRenderer lr;
    private Transform[] points;

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
            Vector3 offsetPosition = new Vector3(points[i].position.x, points[i].position.y + lineOffset, points[i].position.z);
            lr.SetPosition(i, offsetPosition);
        }
    }
}
