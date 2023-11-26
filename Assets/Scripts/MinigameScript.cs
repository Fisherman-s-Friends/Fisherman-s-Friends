using UnityEngine;
using UnityEngine.UI;

public class MinigameScript : MonoBehaviour
{
    [SerializeField] GameObject fishIconObj, catchArea, fishBar, pivotPointObj;
    [SerializeField] Slider slider;
    [SerializeField] FishBehaviour fishBehaviour;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject popUp;

    private RectTransform fishIconTrans, catchAreaTrans, pivotPointTrans;
    private Vector3 pivotPoint;

    private float fishSmoothness, fishSpeedMultiplier, fishSliderSize;
    private float fishPosition, fishDestination, fishTimer, fishSpeed, sliderGainSpeed = 15f;
    private int noPointsDistance, halfPointsDistance, areaDistance;
    public GameObject fishCaught;
    void Start()
    {

        fishIconTrans = fishIconObj.GetComponent<RectTransform>();
        catchAreaTrans = catchArea.GetComponent<RectTransform>();
        pivotPointTrans = pivotPointObj.gameObject.GetComponent<RectTransform>();
        pivotPoint.x = -(pivotPointTrans.localPosition.x + (catchAreaTrans.sizeDelta.x / 2));
        noPointsDistance = Mathf.RoundToInt((fishIconTrans.sizeDelta.x / 2) + (catchAreaTrans.sizeDelta.x / 2));
        halfPointsDistance = Mathf.RoundToInt(noPointsDistance * 0.5f);

        fishSpeedMultiplier = fishBehaviour.FishMoveSpeed() / 10;
        fishSmoothness = fishBehaviour.FishMoveSmoothness() / 10;
        fishSliderSize = fishBehaviour.FishBarValueSize();
        slider.maxValue = fishSliderSize;
    }

    void Update()
    {
        if (slider.value == fishSliderSize)
        {
            // here you could add the splash screen for the fish you caught
            popUp.SetActive(true);
            GameObject fishModel = popUp.transform.Find("FishSpin").gameObject;
            fishModel.GetComponent<MeshFilter>().sharedMesh = fishCaught.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        //  fishModel.GetComponent<MeshRenderer>().material = fishCaught.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;

            playerController.ResetEverything();
        }

        areaDistance = Mathf.RoundToInt(Mathf.Abs(catchAreaTrans.localPosition.x - fishIconTrans.localPosition.x));

        if (areaDistance > noPointsDistance)
            slider.value -= sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance >= halfPointsDistance)
            slider.value += sliderGainSpeed / 2 * Time.deltaTime;
        else if (areaDistance <= halfPointsDistance)
            slider.value += sliderGainSpeed * Time.deltaTime;

        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = Random.value * fishSpeedMultiplier;
            float oldFishDestination = fishDestination;
            fishDestination = Random.value;
            if (fishDestination - oldFishDestination < 0.1f)
                fishDestination += 0.05f;
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
