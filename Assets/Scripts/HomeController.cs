using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGame()
    {
        SceneController.ChangeScene(Scenes.Play);
    }

    public void OnOpenHome()
    {
        Debug.Log("Home is opened");
    }
}
