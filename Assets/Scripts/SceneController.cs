using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static IEnumerator ChangeScene(Scenes scene)
    {
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
     Play = 1
}
