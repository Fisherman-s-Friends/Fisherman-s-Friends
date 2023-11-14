using UnityEngine.SceneManagement;

public static class SceneController
{
    public static void ChangeScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}

 public enum Scenes
 {
     Home = 0,
     Play = 1
}
