using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dialog/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Line> lines;
    public List<Choice> choices;
}

[Serializable]
public class Line
{
    public Actor speaker;
    public string content;
}

[Serializable]
public class Choice
{
    public Line line;
    public string display;
    public Dialogue response;

    public Choice()
    {
        line = new Line();
    }
}
