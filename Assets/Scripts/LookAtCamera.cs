using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        transform.LookAt(new Vector3(0, 0, cam.transform.position.z));
    }

}
