using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialog
{
    public class ChoicesController : MonoBehaviour
    {
        [SerializeField] Transform ChoicesHolder;
        [SerializeField] GameObject ButtonPrefab;

        public Choice selectedChoice;

        public void RenderChoices(IEnumerable<Choice> choices)
        {
            foreach (Choice choice in choices)
            {
                var button = Instantiate(ButtonPrefab, ChoicesHolder);
                button.GetComponentInChildren<TMPro.TMP_Text>().text = choice.display;

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectedChoice = choice;
                    choice.callback.Invoke();
                });
            }
        }
    }

}