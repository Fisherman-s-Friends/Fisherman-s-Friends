using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static bool PlayAnimationOnLoad;

    public static IEnumerator ChangeScene(Scenes scene)
    {
        PlayAnimationOnLoad = true;
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
        
        yield return new WaitForSeconds(1);
        
        var newScene = SceneManager.LoadSceneAsync((int)scene);
        
        while(!newScene.isDone)
        {
            yield return null;
        }
    }
}

 public enum Scenes
 {
     Home = 0,
     Play = 1,
     Lose = 2,
     Winn = 3,
}
