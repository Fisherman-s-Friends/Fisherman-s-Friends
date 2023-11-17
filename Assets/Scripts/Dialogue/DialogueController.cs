using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text textField;
    [SerializeField] private Image speaker;
    [SerializeField] private float typingSpeedInSeconds;
    [SerializeField] private ChoicesController choiceController;

    [SerializeField] private GameObject Box;
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private GameObject ChoiceBox;

    [SerializeField] private Dialogue test;

    private bool skipLine = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDialog(test));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipLine = true;
        }
    }

    public IEnumerator StartDialog(Dialogue dialogue)
    {
        // Set box active
        Box.SetActive(true);
         
        ShowDialogBox();

        // Show all lines
        foreach (var line in dialogue.lines)
        {
            yield return ShowLineAndWaitForInput(line);
        }

        // If dialog box has no choices, Hide the box
        if(dialogue.choices.Count == 0)
        {
            Box.SetActive(false);
            speaker.color = new Color(0, 0, 0, 0);
            yield break;
        }

        ShowChoicesBox();
        
        // Hide speaker
        speaker.color = new Color(0, 0, 0, 0);

        // Presenent the choices and get the selection
        choiceController.RenderChoices(dialogue.choices);
        yield return WaitForSelection(choiceController);
        
        var selected = choiceController.selectedChoice;
        
        ShowDialogBox();

        yield return ShowLineAndWaitForInput(selected.line);

        // Start response dialog to the choice
        StartCoroutine(StartDialog(selected.response));
    }

    private void ShowDialogBox()
    {
        DialogueBox.SetActive(true);
        ChoiceBox.SetActive(false);
    }

    private void ShowChoicesBox()
    {
        DialogueBox.SetActive(false);
        ChoiceBox.SetActive(true);
    }

    private IEnumerator ShowLineAndWaitForInput(Line line)
    {
        yield return WriteLine(line);
        yield return WaitForInput();
    }

    private IEnumerator WriteLine(Line line)
    {
        skipLine = false;
        speaker.sprite = line.speaker?.portrait;
        speaker.color = line.speaker?.portrait ? Color.white : new Color(0,0,0,0);

        textField.text = "";
        foreach (var letter in line.content) {
            if (skipLine)
            {
                textField.text = line.content;
                yield return new WaitForSeconds(typingSpeedInSeconds);
                break;
            }

            textField.text += letter;

            if(letter != ' ')
                yield return new WaitForSeconds(typingSpeedInSeconds);
        }
    }

    private static IEnumerator WaitForInput()
    {
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            yield return null;
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
