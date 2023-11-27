using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDeactivator : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    public void PopUpDisable()
    {
        popUp.SetActive(false);
    }
}
