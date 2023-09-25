using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField]
    private float timerDuration;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration;    
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if( timer < 0 )
        {
            Destroy(gameObject);
        }
    }
}
