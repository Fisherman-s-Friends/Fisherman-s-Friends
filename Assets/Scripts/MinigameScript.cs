using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameScript : MonoBehaviour
{
    [SerializeField] private GameObject fishIconObj, areaObj;
    private Transform fishPos, areaPos;

    private void Start()
    {        
        fishPos = fishIconObj.GetComponent<Transform>();
        areaPos = areaObj.GetComponent<Transform>();
        Debug.Log(areaPos + " ++++" + fishPos);
    }

    void Update()
    {

    }
}
