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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDialog(test));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartDialog(Dialogue dialogue)
    {
        // Set box active
        Box.SetActive(true);
         
        // Set dialogue box active and choice box inactive
        DialogueBox.SetActive(true);
        ChoiceBox.SetActive(false);

        // Show all lines
        foreach (var line in dialogue.lines)
        {
            yield return WriteLine(line);
            yield return WaitForInput();
        }

        // If dialog box has no choices, Hide the box
        if(dialogue.choices.Count == 0)
        {
            Box.SetActive(false);
            speaker.color = new Color(0, 0, 0, 0);
            yield break;
        }

        // Set dialogue box inactive and choice box active
        DialogueBox.SetActive(false);
        ChoiceBox.SetActive(true);
        
        // Hide speaker
        speaker.color = new Color(0, 0, 0, 0);

        // Presenent the choices and get the selection
        choiceController.RenderChoices(dialogue.choices);
        yield return WaitForSelection(choiceController);
        
        var selected = choiceController.selectedChoice;

        // Set dialogue box active and choice box inactive
        DialogueBox.SetActive(true);
        ChoiceBox.SetActive(false);

        // Show selection full line
        yield return WriteLine(selected.line);
        yield return WaitForInput();

        // Start response dialog to the choice
        StartCoroutine(StartDialog(selected.response));
    }

    private IEnumerator WriteLine(Line line)
    {
        speaker.sprite = line.speaker?.portrait;
        speaker.color = line.speaker?.portrait ? Color.white : new Color(0,0,0,0);

        textField.text = "";
        foreach (var letter in line.content) { 
            textField.text += letter;
            if(letter != ' ')
                yield return new WaitForSeconds(typingSpeedInSeconds);
        }
    }

    private static IEnumerator WaitForInput()
    {
        while (true)
        {
            if(Input.GetKey(KeyCode.Space))
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
