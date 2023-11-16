using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "Dialog/Choice")]
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
