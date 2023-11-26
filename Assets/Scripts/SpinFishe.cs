using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFishe : MonoBehaviour
{
    public float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.localScale += new Vector3(rotationSpeed, rotationSpeed, rotationSpeed) * Time.deltaTime;
    }
}
