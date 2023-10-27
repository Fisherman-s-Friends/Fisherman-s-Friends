using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionController : MonoBehaviour
{
    static private GameObject sessionObject;

    [SerializeField] private int money;
    void Start()
    {
        if (sessionObject != null)
        {
            Destroy(this);
            return;
        }

        sessionObject = gameObject;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    
}
