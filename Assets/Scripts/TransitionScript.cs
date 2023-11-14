using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneController.PlayAnimationOnLoad)
            GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
