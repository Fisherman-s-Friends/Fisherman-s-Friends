using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MinigameScript : MonoBehaviour
{
    [SerializeField] private GameObject fishIconObj, catchArea, fishBar, pivotPointObj;
    [SerializeField] private Slider slider;
    [SerializeField] private FishBehaviour fishBehaviour;
    private float fishSmoothness, fishSpeedMultiplier;
    private RectTransform fishIconTrans, catchAreaTrans, pivotPointTrans;
    private Vector3 pivotPoint;
    private float sliderGainSpeed = 20, fishPosition, fishDestination, fishTimer, fishSpeed;
    private int noPointsDistance, halfPointsDistance;

    void Start()
    {
        fishIconTrans = fishIconObj.GetComponent<RectTransform>();
        catchAreaTrans = catchArea.GetComponent<RectTransform>();
        pivotPointTrans = pivotPointObj.gameObject.GetComponent<RectTransform>();
        pivotPoint.x = -(pivotPointTrans.localPosition.x + (catchAreaTrans.sizeDelta.x / 2));
        noPointsDistance = Mathf.RoundToInt((fishIconTrans.sizeDelta.x / 2) + (catchAreaTrans.sizeDelta.x / 2));
        halfPointsDistance = Mathf.RoundToInt(noPointsDistance * 0.5f);
    }
    void Update()
    {
        int areaDistance = Mathf.RoundToInt(Mathf.Abs(catchAreaTrans.localPosition.x - fishIconTrans.localPosition.x));

        if (areaDistance > noPointsDistance)
            slider.value -= sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance >= halfPointsDistance)
            slider.value += sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance <= halfPointsDistance)
            slider.value += sliderGainSpeed * Time.deltaTime;

        fishSpeedMultiplier = fishBehaviour.FishMoveSpeed() / 10;
        fishSmoothness = fishBehaviour.FishMoveSmoothness() / 10;

        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = Random.value * fishSpeedMultiplier;
            fishDestination = Random.value;
        }

        if (fishPosition < fishDestination)
            fishIconTrans.transform.localScale = new Vector3(-1f, 1, 1);
        else
            fishIconTrans.transform.localScale = new Vector3(1f, 1, 1);

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, fishSmoothness);
        fishIconTrans.localPosition =
            Vector3.Lerp
            (new Vector3(-pivotPoint.x, fishIconTrans.localPosition.y),
            new Vector3(pivotPoint.x, fishIconTrans.localPosition.y),
            fishPosition);


    }

    public void GetFishBehaviour(FishBehaviour currentBehaviour)
    {
        fishBehaviour = currentBehaviour;
    }

    public void MinigameMovement(Vector2 direction, float speed)
    {
        if (catchAreaTrans != null)
        {
            catchAreaTrans.localPosition = new Vector2(catchAreaTrans.localPosition.x + Time.deltaTime * direction.x * speed, catchAreaTrans.localPosition.y);
            if (catchAreaTrans.localPosition.x > pivotPoint.x)
                catchAreaTrans.localPosition = new Vector2(pivotPoint.x, catchAreaTrans.localPosition.y);
            else if (catchAreaTrans.localPosition.x < -pivotPoint.x)
                catchAreaTrans.localPosition = new Vector2(-pivotPoint.x, catchAreaTrans.localPosition.y);
        }
    }
}
