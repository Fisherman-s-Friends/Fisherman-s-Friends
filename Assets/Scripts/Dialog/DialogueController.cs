using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Dialog
{
    [RequireComponent(
        typeof(IDialogWindowManager),
        typeof(IDialogInput),
        typeof(IWriter))]
    public class DialogueController : MonoBehaviour
    {

        [SerializeField] private Dialogue test;

        private IDialogWindowManager windowManager;
        private IDialogInput input;
        private IWriter writer;
        private ISpeakerController speakerController;
        private IChoiceController choiceController;

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

            if (!TryGetComponent(out writer))
            {
                throw new ArgumentNullException("writer",
                    "Couldn't find IWriter attached to the game object");
            }

            speakerController = GetComponentInChildren<ISpeakerController>(true);
            choiceController = GetComponentInChildren<IChoiceController>(true);
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
                writer.SkipLine();
            }
        }

        public IEnumerator StartDialog(Dialogue dialogue)
        {
            windowManager.ToggleDialogWindow(true);

            windowManager.ShowDialogBox();

            foreach (var line in dialogue.lines)
            {
                yield return ShowLineAndWaitForInput(line);
            }

            if (dialogue.choices.Count == 0 || choiceController == null)
            {
                windowManager.ToggleDialogWindow(false);
                speakerController?.UpdateSpeaker(null);
                yield break;
            }

            windowManager.ShowChoicesBox();

            speakerController?.UpdateSpeaker(null);

            choiceController.AskToChoises(dialogue);
            yield return choiceController.WaitForSelection();

            var selected = choiceController.SelectedChoice;

            windowManager.ShowDialogBox();

            yield return ShowLineAndWaitForInput(selected.line);

            StartCoroutine(StartDialog(selected.response));
        }

        private IEnumerator ShowLineAndWaitForInput(Line line)
        {
            yield return WriteLine(line);
            yield return input.WaitForInput();
        }

        private IEnumerator WriteLine(Line line)
        {
            speakerController?.UpdateSpeaker(line.speaker);

            yield return writer.WriteByCharacter(line.content);
        }
    }
}