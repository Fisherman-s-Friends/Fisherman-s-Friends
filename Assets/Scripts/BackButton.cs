using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void ButtonPressed()
    {
       StartCoroutine(SceneController.ChangeScene(Scenes.Home));
    }
}
