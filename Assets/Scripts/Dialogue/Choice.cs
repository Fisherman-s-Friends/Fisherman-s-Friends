using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Choice : ScriptableObject
{
    public Line line;
    public string display;
    public Dialogue response;
    public UnityEvent callback;

    public Choice()
    {
        line = new Line();
    }
}
