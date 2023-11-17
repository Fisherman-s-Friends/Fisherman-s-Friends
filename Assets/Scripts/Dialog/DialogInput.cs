using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public class DialogInput : MonoBehaviour, IDialogInput
    {
        [SerializeField] private KeyCode[] keys;

        public bool GetInput()
        {
            return keys.Any(key => Input.GetKeyDown(key)) || Input.GetMouseButtonDown(0);
        }

        public IEnumerator WaitForInput()
        {
            while (true)
            {
                if (GetInput())
                {
                    break;
                }
                yield return null;
            }
        }
    }
}