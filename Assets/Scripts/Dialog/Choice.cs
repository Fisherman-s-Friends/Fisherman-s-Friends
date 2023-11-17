using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog
{
    [CreateAssetMenu(fileName = "Data", menuName = "Dialog/Choice")]
    public class Choice : ScriptableObject
    {
        public Line line;
        public string display;
        public Dialogue response;

        [HideInInspector]
        public UnityEvent callback;
    }

}