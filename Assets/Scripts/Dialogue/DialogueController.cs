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
        Box.SetActive(true);
        DialogueBox.SetActive(true);
        ChoiceBox.SetActive(false);

        foreach (var line in dialogue.lines)
        {
            yield return WriteLine(line);
            yield return WaitForInput();
        }

        if(dialogue.choices.Count == 0)
        {
            WaitForInput();
            Box.SetActive(false);
            speaker.color = new Color(0, 0, 0, 0);
            yield break;
        }

        DialogueBox.SetActive(false);
        ChoiceBox.SetActive(true);
        speaker.color = new Color(0, 0, 0, 0);

        choiceController.RenderChoices(dialogue.choices);
        yield return WaitForSelection(choiceController);
        
        var selected = choiceController.selectedChoice;
        
        WriteLine(selected.line);
        WaitForInput();
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
