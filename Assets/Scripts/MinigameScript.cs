using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MinigameScript : MonoBehaviour
{
    [SerializeField] private GameObject fishIconObj, catchArea, fishBar;
    [SerializeField] private Slider slider;
    private Transform fishIconTrans, catchAreaTrans;
    float sliderGainSpeed = 10;

    private void Start()
    {
        fishIconTrans = fishIconObj.GetComponent<Transform>();
        catchAreaTrans = catchArea.GetComponent<Transform>();
    }

    void Update()
    {
        if (fishIconTrans.position.x == catchAreaTrans.position.x)
            slider.value += sliderGainSpeed * Time.deltaTime;
        if (slider.value == 0)
            return;
        else
            slider.value -= sliderGainSpeed / 2 * Time.deltaTime;
    }

    public void MinigameMovement(Vector2 movementDir, float controlSpeed)
    {

        // Default value was above 333 and below  negative 333
        if (catchArea.transform.localPosition.x >= 333f && movementDir.x == 1) { return; }
        else if (catchArea.transform.localPosition.x <= -333f && movementDir.x == -1) { return; }
        catchArea.transform.localPosition = new Vector2(catchArea.transform.localPosition.x + movementDir.x * (controlSpeed), catchArea.transform.localPosition.y);
    }
}
