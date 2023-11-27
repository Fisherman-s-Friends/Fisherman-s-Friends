using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    public GameObject splashScreen;
    public float secondsToWaitForFish;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (splashScreen != null && splashScreen.activeSelf == true)
        {
            StartCoroutine(WaitToDisable());
        }
    }

    IEnumerator WaitToDisable()
    {
        playerController.ResetEverything();
        yield return new WaitForSeconds(secondsToWaitForFish);
        splashScreen.SetActive(false);
    }
}
