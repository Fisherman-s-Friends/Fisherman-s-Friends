using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class DialogWindowManager : MonoBehaviour, IDialogWindowManager
    {
        [SerializeField] private GameObject Box;
        [SerializeField] private GameObject DialogueBox;
        [SerializeField] private GameObject ChoiceBox;

        public void ToggleDialogWindow(bool visible)
        {
            Box.SetActive(visible);
        }

        public void ShowDialogBox()
        {
            DialogueBox.SetActive(true);
            ChoiceBox.SetActive(false);
        }

        public void ShowChoicesBox()
        {
            DialogueBox.SetActive(false);
            ChoiceBox.SetActive(true);
        }
    }
}