using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Selectable : MonoBehaviour
{
    Outline outline;

    [SerializeField]
    UnityEvent onClick;

    [SerializeField]
    float pulseSpeed;

    [SerializeField]
    float masPulseWidth;

    [SerializeField, Range(0,1)]
    float pulseAlpha;

    private bool hoveredOn;

    // Start is called before the first frame update
    void Start()
    {
        hoveredOn = false;
        outline = GetComponent<Outline>();
        outline.OutlineColor = ChangeOutlineColor(outline.OutlineColor, pulseAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        if(!hoveredOn)
        {
            outline.OutlineWidth = Mathf.PingPong(Time.time * pulseSpeed, masPulseWidth);
        }
    }

    private void OnMouseOver()
    {
        hoveredOn = true;
        outline.OutlineWidth = 10;
        outline.OutlineColor = ChangeOutlineColor(outline.OutlineColor, 1);
    }

    private void OnMouseUp()
    {
        onClick.Invoke();
    }

    private void OnMouseExit()
    {
        hoveredOn = false;
        outline.OutlineColor = ChangeOutlineColor(outline.OutlineColor, pulseAlpha);
    }

    private Color ChangeOutlineColor (Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
