using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinFishe : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = startScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.localScale += new Vector3(rotationSpeed, rotationSpeed, rotationSpeed) * Time.deltaTime;
    }
}
