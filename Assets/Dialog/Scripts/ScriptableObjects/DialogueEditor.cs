using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialog
{
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

            try
            {
                RenderDialogueList();

                EditorGUILayout.BeginVertical();
                try
                {
                    if (!selectedDialogue.dialogue)
                    {
                        GUILayout.Label("No dialogue selected");
                        return;
                    }

                    GUILayout.Label(selectedDialogue.path, EditorStyles.whiteLargeLabel);

                    #region Lines

                    GUILayout.Label($"Lines: {selectedDialogue.dialogue.lines.Count}", EditorStyles.whiteLargeLabel);

                    linesScrollPos = EditorGUILayout.BeginScrollView(linesScrollPos);
                    try
                    {
                        foreach (var line in selectedDialogue.dialogue.lines)
                        {
                            if (DrawLineEditor(line, () => { selectedDialogue.dialogue.lines.Remove(line); }))
                            {
                                return;
                            }
                            EditorGUILayout.Space(10f);
                        }
                    }
                    finally
                    {
                        EditorGUILayout.EndScrollView();
                    }

                    AddButton("Add new line", () =>
                    {
                        selectedDialogue.dialogue.lines.Add(new Line());
                    });

                    #endregion

                    #region Choices

                    GUILayout.Label($"Choices: {selectedDialogue.dialogue.choices.Count}", EditorStyles.whiteLargeLabel);

                    choicesScrollPos = EditorGUILayout.BeginScrollView(choicesScrollPos);
                    try
                    {
                        foreach (var choice in selectedDialogue.dialogue.choices)
                        {
                            EditorGUILayout.BeginHorizontal();

                            GUILayout.Label("Display");
                            choice.display = EditorGUILayout.TextField(choice.display);

                            EditorGUILayout.EndHorizontal();

                            if (DrawLineEditor(choice.line, () => { selectedDialogue.dialogue.choices.Remove(choice); }))
                            {
                                return;
                            }

                            EditorGUILayout.BeginHorizontal();

                            GUILayout.Label("Response");
                            choice.response = EditorGUILayout.ObjectField("", choice.response, typeof(Dialogue), allowSceneObjects: false) as Dialogue;

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space(10f);
                        }
                    }
                    finally
                    {
                        EditorGUILayout.EndScrollView();
                    }

                    EditorGUILayout.BeginHorizontal();

                    AddButton("Add new existing choice", () =>
                    {
                        EditorGUIUtility.ShowObjectPicker<Choice>(null, false, "t:Choice", 0);
                    });

                    if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == 0)
                    {
                        var selected = EditorGUIUtility.GetObjectPickerObject();
                        if (selected != null && selected is Choice)
                        {
                            selectedDialogue.dialogue.choices.Add(selected as Choice);
                        }
                    }

                    AddButton("Add new choice", () =>
                    {
                        var choice = CreateInstance<Choice>();
                        AssetDatabase.CreateAsset(choice, $"Assets/{selectedDialogue.dialogue.name}Choice_{selectedDialogue.dialogue.choices.Count}.asset");
                        selectedDialogue.dialogue.choices.Add(choice);
                    });

                    EditorGUILayout.EndHorizontal();

                    #endregion

                }
                finally
                {
                    EditorGUILayout.EndVertical();
                }
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Renders a list, where dialog items are listed as links
        /// </summary>
        private void RenderDialogueList()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(200));

            GUILayout.Label("Dialog items");

            EditorGUILayout.BeginScrollView(listScrollPos);

            foreach (var dialogListItem in dialogueList)
            {
                if (EditorGUILayout.LinkButton(dialogListItem.path))
                {
                    selectedDialogue = dialogListItem;
                }
            }

            EditorGUILayout.EndScrollView();

            AddButton("New", () =>
            {
                AssetDatabase.CreateAsset(CreateInstance<Dialogue>(), $"Assets/New Dialogue {dialogueList.Count}.asset");
                dialogueList = LoadAssets<Dialogue>("t:Dialogue");
            });

            AddButton("Fetch assets", () =>
            {
                dialogueList = LoadAssets<Dialogue>("t:Dialogue");
            });

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Add a button
        /// </summary>
        /// <param name="txt">Button text</param>
        /// <param name="callBack">Callback, when button is pressed</param>
        private void AddButton(string txt, Action callBack)
        {
            if (GUILayout.Button(txt))
            {
                callBack();
            }
        }

        /// <summary>
        /// Render an editor for a line
        /// </summary>
        /// <param name="line">Line</param>
        /// <param name="removeButtonAction">Callback, when the remove button is pressed</param>
        /// <returns></returns>
        private bool DrawLineEditor(Line line, Action removeButtonAction)
        {
            EditorGUILayout.BeginHorizontal("box");

            if (line.speaker?.portrait?.texture)
                GUILayout.Label(line.speaker.portrait.texture, GUILayout.Width(100), GUILayout.Height(100));

            line.speaker = EditorGUILayout.ObjectField("", line.speaker, typeof(Actor), allowSceneObjects: false) as Actor;

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                removeButtonAction();
                EditorGUILayout.EndHorizontal();
                return true;
            }

            EditorGUILayout.EndHorizontal();

            line.content = EditorGUILayout.TextArea(line.content, GUILayout.Height(100));
            return false;
        }

        /// <summary>
        /// Load assets of type 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filter">Filter by which to load</param>
        /// <returns>A list of paths and items</returns>
        private static List<(string path, T asset)> LoadAssets<T>(string filter) where T : UnityEngine.Object
        {
            var GUIDs = AssetDatabase.FindAssets(filter);
            List<(string path, T asset)> assets = new List<(string path, T asset)>();
            foreach (var GUID in GUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(GUID);
                assets.Add((path, AssetDatabase.LoadAssetAtPath<T>(path)));
            }
            return assets;
        }
    } 
}