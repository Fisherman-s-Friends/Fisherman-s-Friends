using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using static UnityEditor.Progress;

public class DialogueEditor : EditorWindow
{
    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<DialogueEditor>();
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
            AssetDatabase.CreateAsset(new Dialogue(), $"Assets/New Dialogue {dialogueList.Count}.asset");
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

        if (GUILayout.Button("Add new choice"))
        {
            selectedDialogue.dialogue.choices.Add(new Choice());
        };

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
