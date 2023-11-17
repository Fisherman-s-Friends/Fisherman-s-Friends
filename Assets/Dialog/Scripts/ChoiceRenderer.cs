using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class ChoiceRenderer : MonoBehaviour, IChoiceRenderer
    {
        [SerializeField] Transform ChoicesHolder;
        [SerializeField] GameObject ButtonPrefab;

        public void RenderChoices(IEnumerable<Choice> choices, Action<Choice> callback)
        {
            foreach (Choice choice in choices)
            {
                var button = Instantiate(ButtonPrefab, ChoicesHolder);
                button.GetComponentInChildren<TMP_Text>().text = choice.display;

                button.GetComponent<Button>().onClick.AddListener(() => callback(choice));
            }
        }
    }
}