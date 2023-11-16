using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using static UnityEditor.Progress;

public class DialogueEditor : EditorWindow
{
    [MenuItem("Window/Dialogue")]
    public static DialogueEditor ShowWindow()
    {
        return GetWindow<DialogueEditor>();
    }

    [OnOpenAssetAttribute]
    public static bool OpenDialogAsset(int instanceId, int line)
    {
        var instance = EditorUtility.InstanceIDToObject(instanceId);

        if (instance is not Dialogue) return false;

        ShowWindow().selectedDialogue = (AssetDatabase.GetAssetPath(instance), instance as Dialogue);

        return true;
    }

    private List<(string path, Dialogue dialogue)> dialogueList = new List<(string path, Dialogue dialogue)>();

    private (string path, Dialogue dialogue) selectedDialogue;

    private Vector2 listScrollPos = Vector2.zero, linesScrollPos = Vector2.zero, choicesScrollPos = Vector2.zero;

    private void Awake()
    {
        dialogueList = LoadAssets<Dialogue>("t:Dialogue");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        #region Dialogue list
        EditorGUILayout.BeginVertical("box",GUILayout.Width(200));

        GUILayout.Label("Dialog items");

        EditorGUILayout.BeginScrollView(listScrollPos);

        foreach (var dialogListItem in dialogueList)
        {
            if(EditorGUILayout.LinkButton(dialogListItem.path))
            {
                selectedDialogue = dialogListItem;
            }
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("New")) 
        { 
            AssetDatabase.CreateAsset(CreateInstance<Dialogue>(), $"Assets/New Dialogue {dialogueList.Count}.asset");
            dialogueList = LoadAssets<Dialogue>("t:Dialogue");
        }

        if (GUILayout.Button("Fetch assets"))
        {
            dialogueList = LoadAssets<Dialogue>("t:Dialogue");
        }

        EditorGUILayout.EndVertical();
        #endregion

        #region Editor layout

        if(!selectedDialogue.dialogue)
        {
            GUILayout.Label("No dialogue selected");
            EditorGUILayout.EndHorizontal();
            return;
        }

        EditorGUILayout.BeginVertical();

        GUILayout.Label(selectedDialogue.path, EditorStyles.whiteLargeLabel);

        #region Lines

        GUILayout.Label($"Lines: {selectedDialogue.dialogue.lines.Count}", EditorStyles.whiteLargeLabel);

        linesScrollPos = EditorGUILayout.BeginScrollView(linesScrollPos);

        foreach (var line in selectedDialogue.dialogue.lines)
        {
            EditorGUILayout.BeginHorizontal("box");

            if(line.speaker?.portrait?.texture)
                GUILayout.Label(line.speaker.portrait.texture, GUILayout.Width(100), GUILayout.Height(100));

            line.speaker = EditorGUILayout.ObjectField("", line.speaker, typeof(Actor), allowSceneObjects: false) as Actor;

            if(GUILayout.Button("-", GUILayout.Width(20)))
            {
                selectedDialogue.dialogue.lines.Remove(line);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                return;
            }

            EditorGUILayout.EndHorizontal();

            line.content = EditorGUILayout.TextArea(line.content, GUILayout.Height(100));
            EditorGUILayout.Space(10f);
        }

        EditorGUILayout.EndScrollView();

        if(GUILayout.Button("Add new line"))
        {
            selectedDialogue.dialogue.lines.Add(new Line());
        }

        #endregion

        #region Choices

        GUILayout.Label($"Choices: {selectedDialogue.dialogue.choices.Count}", EditorStyles.whiteLargeLabel);

        choicesScrollPos = EditorGUILayout.BeginScrollView(choicesScrollPos);

        foreach(var choice in selectedDialogue.dialogue.choices)
        {
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label("Display");
            choice.display = EditorGUILayout.TextField(choice.display);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");

            GUILayout.Label("Line");

            choice.line.speaker = EditorGUILayout.ObjectField("", choice.line.speaker, typeof(Actor), allowSceneObjects: false) as Actor;

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                selectedDialogue.dialogue.choices.Remove(choice);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                return;
            }

            EditorGUILayout.EndHorizontal();

            choice.line.content = EditorGUILayout.TextArea(choice.line.content, GUILayout.Height(100));

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Response");
            choice.response = EditorGUILayout.ObjectField("", choice.response, typeof(Dialogue), allowSceneObjects: false) as Dialogue;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add new existing choice"))
        {
            EditorGUIUtility.ShowObjectPicker<Choice>(null, false, "t:Choice", 0);
        };
        if (GUILayout.Button("Add new choice"))
        {
            var choice = CreateInstance<Choice>();
            AssetDatabase.CreateAsset(choice, $"Assets/{selectedDialogue.dialogue.name}Choice_{selectedDialogue.dialogue.choices.Count}.asset");
            selectedDialogue.dialogue.choices.Add(choice);
        };
        EditorGUILayout.EndHorizontal();

        if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == 0)
        {

            var selected = EditorGUIUtility.GetObjectPickerObject();
            if (selected != null && selected is Choice)
            {
                selectedDialogue.dialogue.choices.Add(selected as Choice);
            }
        }
        /*
        var choice = new Choice();
        AssetDatabase.CreateAsset(choice, $"Assets/{selectedDialogue.dialogue.name}Choice_{selectedDialogue.dialogue.choices.Count}.asset");
        selectedDialogue.dialogue.choices.Add(choice);
        */

        #endregion

        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.EndHorizontal();
    }

    private static List<(string path, T asset)> LoadAssets<T>(string filter) where T : UnityEngine.Object
    {
        var GUIDs = AssetDatabase.FindAssets(filter);
        List<(string path, T asset)> assets = new List<(string path, T asset)>();
        foreach (var GUID in GUIDs) {
            var path = AssetDatabase.GUIDToAssetPath(GUID);
            assets.Add((path, AssetDatabase.LoadAssetAtPath<T>(path)));
        }
        return assets;
    }
}