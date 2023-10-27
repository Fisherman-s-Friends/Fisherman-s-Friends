using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MinigameScript : MonoBehaviour
{
    [SerializeField] private GameObject fishIconObj, catchArea, fishBar;
    [SerializeField] private Slider slider;
    private RectTransform fishIconTrans, catchAreaTrans;
    private float sliderGainSpeed = 10, boundaries=333f;
    private int noPointDistance, halfPointDistance;
    [SerializeField]private FishBehaviour fishBehaviour;
    private void Start()
    {
        fishIconTrans = fishIconObj.GetComponent<RectTransform>();
        catchAreaTrans = catchArea.GetComponent<RectTransform>();

        noPointDistance = Mathf.RoundToInt((fishIconTrans.sizeDelta.x / 2) + (catchAreaTrans.sizeDelta.x / 2));
        halfPointDistance = Mathf.RoundToInt(noPointDistance * 0.5f);
    }

    // use this when fish gets hooked, to determine the fish-behaviour used in the minigame
    public void StartMinigame(FishBehaviour currentBehaviour)
    {
        fishBehaviour = currentBehaviour;
    }

    void Update()
    {
        int areaDistance = Mathf.RoundToInt(Mathf.Abs(catchAreaTrans.localPosition.x - fishIconTrans.localPosition.x));

        if (areaDistance > noPointDistance)
            slider.value -= sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance >= halfPointDistance)
            slider.value += sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance <= halfPointDistance)
            slider.value += sliderGainSpeed * Time.deltaTime;

        fishIconTrans.localPosition = new Vector2((fishBehaviour.FishMovement(fishIconTrans.localPosition)), fishIconTrans.localPosition.y);
    }

    public void MinigameMovement(Vector2 movementDir, float controlSpeed)
    {
        catchAreaTrans.localPosition = new Vector2(catchAreaTrans.localPosition.x + Time.deltaTime * movementDir.x * controlSpeed, catchAreaTrans.localPosition.y);
        if (catchAreaTrans.localPosition.x > boundaries)
            catchAreaTrans.localPosition = new Vector2(boundaries, catchAreaTrans.localPosition.y);
        else if (catchAreaTrans.localPosition.x < -boundaries)
            catchAreaTrans.localPosition = new Vector2(-boundaries, catchAreaTrans.localPosition.y);
    }


}
