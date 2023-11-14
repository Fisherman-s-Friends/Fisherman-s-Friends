using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    [SerializeField]
    private bool startWithAnim = false;
    // Start is called before the first frame update
    void Start()
    {
        if (startWithAnim)
            GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
