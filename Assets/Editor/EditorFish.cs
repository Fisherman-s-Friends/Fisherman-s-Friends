using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FishController))]
public class EditorFish : Editor
{
    int CurrentTab;
    #region Serialized Properties
    SerializedProperty fishBoundingBoxOffset;
    SerializedProperty fishBoundingBoxSize;
    SerializedProperty fishObjects;
    SerializedProperty timeToSpawn;
    SerializedProperty currentTimeToSpawn;
    SerializedProperty countFish;
    SerializedProperty maxFish;
    #endregion

    private void OnEnable()
    {
        #region Serialized find properties
        fishBoundingBoxOffset = serializedObject.FindProperty("fishBoundingBoxOffset");
        fishBoundingBoxSize = serializedObject.FindProperty("fishBoundingBoxSize");
        fishObjects = serializedObject.FindProperty("fishObjects");
        timeToSpawn = serializedObject.FindProperty("timeToSpawn");
        currentTimeToSpawn = serializedObject.FindProperty("currentTimeToSpawn");
        countFish = serializedObject.FindProperty("countFish");
        maxFish = serializedObject.FindProperty("maxFish");
        #endregion
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        CurrentTab = GUILayout.Toolbar(CurrentTab, new string[] { "Fish", "Spawner" });
        switch (CurrentTab)
        {
            case 0:
                EditorGUILayout.PropertyField(fishBoundingBoxOffset);
                EditorGUILayout.PropertyField(fishBoundingBoxSize);
                break;
            case 1:
                EditorGUILayout.PropertyField(fishObjects);
                EditorGUILayout.PropertyField(timeToSpawn);
                EditorGUILayout.PropertyField(currentTimeToSpawn);
                EditorGUILayout.PropertyField(countFish);
                EditorGUILayout.PropertyField(maxFish);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
