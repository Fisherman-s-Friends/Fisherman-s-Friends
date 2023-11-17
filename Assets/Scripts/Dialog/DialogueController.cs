using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    [RequireComponent(
        typeof(IDialogWindowManager), 
        typeof(IDialogInput))]
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text textField;
        [SerializeField] private Image speaker;
        [SerializeField] private float typingSpeedInSeconds;
        [SerializeField] private ChoicesController choiceController;

        [SerializeField] private Dialogue test;

        private IDialogWindowManager windowManager;
        private IDialogInput input;

        private bool skipLine = false;

        private void Awake()
        {
            if (!TryGetComponent(out windowManager))
            {
                throw new ArgumentNullException("windowManager",
                    "Couldn't find IDialogWindowManager attached to the game object");
            }

            if (!TryGetComponent(out input))
            {
                throw new ArgumentNullException("input",
                    "Couldn't find IDialogWindowManager attached to the game object");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartDialog(test));
        }

        // Update is called once per frame
        void Update()
        {
            if (input.GetInput())
            {
                skipLine = true;
            }
        }

        public IEnumerator StartDialog(Dialogue dialogue)
        {
            windowManager.ToggleDialogWindow(true);

            windowManager.ShowDialogBox();

            // Show all lines
            foreach (var line in dialogue.lines)
            {
                yield return ShowLineAndWaitForInput(line);
            }

            // If dialog box has no choices, Hide the box
            if (dialogue.choices.Count == 0)
            {
                windowManager.ToggleDialogWindow(false);
                speaker.color = new Color(0, 0, 0, 0);
                yield break;
            }

            windowManager.ShowChoicesBox();

            // Hide speaker
            speaker.color = new Color(0, 0, 0, 0);

            // Presenent the choices and get the selection
            choiceController.RenderChoices(dialogue.choices);
            yield return WaitForSelection(choiceController);

            var selected = choiceController.selectedChoice;

            windowManager.ShowDialogBox();

            yield return ShowLineAndWaitForInput(selected.line);

            // Start response dialog to the choice
            StartCoroutine(StartDialog(selected.response));
        }

        private IEnumerator ShowLineAndWaitForInput(Line line)
        {
            yield return WriteLine(line);
            yield return input.WaitForInput();
        }

        private IEnumerator WriteLine(Line line)
        {
            skipLine = false;
            speaker.sprite = line.speaker?.portrait;
            speaker.color = line.speaker?.portrait ? Color.white : new Color(0, 0, 0, 0);

            textField.text = "";
            foreach (var letter in line.content)
            {
                if (skipLine)
                {
                    textField.text = line.content;
                    yield return new WaitForSeconds(typingSpeedInSeconds);
                    break;
                }

                textField.text += letter;

                if (letter != ' ')
                    yield return new WaitForSeconds(typingSpeedInSeconds);
            }
        }

        public static IEnumerator WaitForSelection(ChoicesController choicesController)
        {
            while (!choicesController.selectedChoice)
            {
                yield return null;
            }
        }
    }

}