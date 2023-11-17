using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog
{
    [RequireComponent(typeof(IChoiceRenderer))]
    public class ChoiceController : MonoBehaviour, IChoiceController
    {
        private ChoiceRenderer choiceRenderer;

        private void Awake()
        {
            if (!TryGetComponent(out choiceRenderer))
            {
                throw new ArgumentNullException("choiceRenderer",
                    "Couldn't find IChoiceRenderer attached to the game object");
            }
        }

        public Choice SelectedChoice { get; private set; }

        public void AskToChoises(Dialogue dialogue)
        {
            SelectedChoice = null;
            choiceRenderer.RenderChoices(dialogue.choices, (choice) =>
            {
                SelectedChoice = choice;
                choice.callback.Invoke();
            });
        }

        public IEnumerator WaitForSelection()
        {
            while (!SelectedChoice)
            {
                yield return null;
            }
        }
    }

}