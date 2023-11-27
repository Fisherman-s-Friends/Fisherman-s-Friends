using UnityEngine;
using UnityEngine.Events;

public class NightTimer : MonoBehaviour
{
    [SerializeField]
    private float sessionLengthInSeconds;

    private float timeLeft;

    [SerializeField] private TMPro.TMP_Text timeText;
    [SerializeField] private GameObject sky;
    private Material skyMaterial;
    [SerializeField] private Light light;
    [SerializeField]
    private bool debug = false;

    private bool returnHomeCalled = false;
    
    public UnityEvent<float> timer60;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = sessionLengthInSeconds;
        skyMaterial = sky.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            ReturnHome();
            return;
        }

        timeText.SetText(timeLeft < 60 ? timeLeft.ToString("#0.0") : Mathf.Ceil(timeLeft / 60).ToString("#"));
        skyMaterial.mainTextureOffset = new Vector2(0.3f * Mathf.Pow((1 - timeLeft / sessionLengthInSeconds), 2), 0);
        light.intensity = Mathf.Clamp(Mathf.Pow((timeLeft / sessionLengthInSeconds), 0.5f), 0.5f, 1);
        light.color = Color.Lerp(Color.white, new Color(200f / 255, 111f / 255, 0), 1 - timeLeft / sessionLengthInSeconds);
        if (timeLeft < 60 && timeLeft > 59.9f)
        {
            if(timer60 != null) timer60.Invoke(timeLeft);
        }
    }

    private void ReturnHome()
    {
        if (returnHomeCalled) return;
        StartCoroutine(SceneController.ChangeScene(Scenes.Home));
        returnHomeCalled = true;
    }
}
