using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    private SessionController sessionController;
    // Start is called before the first frame update
    void Start()
    {
        sessionController = GameObject.Find("SceneManager").GetComponent<SessionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGame()
    {
        StartCoroutine(SceneController.ChangeScene(Scenes.Play));
    }

    public void OnOpenHome()
    {
        sessionController.NextDay();
    }

    public void OnExitGame()
    {
        Application.Quit();
        Debug.Log("Quit the game");
    }
}
