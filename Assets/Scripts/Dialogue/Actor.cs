using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dialog/Actor")]
public class Actor : ScriptableObject
{
    public Sprite portrait;
    public new string name;
}
