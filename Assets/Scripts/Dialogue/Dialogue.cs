using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dialog/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Line> lines = new List<Line>();
    public List<Choice> choices = new List<Choice>();
}

[Serializable]
public class Line
{
    public Actor speaker;
    public string content;
}
