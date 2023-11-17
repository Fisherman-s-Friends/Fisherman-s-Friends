using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionController : MonoBehaviour
{
    static private GameObject sessionObject;

    [SerializeField] private int loan;
    [SerializeField] private int money;

    [SerializeField]
    private int paybackRate;

    [SerializeField] private int DayCount;
    void Start()
    {
        if (sessionObject != null)
        {
            Destroy(this);
            return;
        }

        sessionObject = gameObject;
        DontDestroyOnLoad(sessionObject);

        UpdateText();
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void Update()
    {

    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateText();
    }

    public void NextDay()
    {
        money -= paybackRate;
        if (money < 0)
        {
            throw new NotImplementedException("Not enough money, you lose");
            //return;
        }
        loan -= paybackRate;
        if (loan <= 0)
        {
            throw new NotImplementedException("Loan payed, you win");
            //return;
        }

        UpdateText();
        UpdateDayCount();
    }
    public void UpdateDayCount()
    {
        DayCount++;
    }

    private void UpdateText()
    {
        var text = GameObject.FindGameObjectWithTag("moneyText");
        if (text != null)
            text.GetComponent<TMPro.TMP_Text>().text = "Value of Fish: " + money;
        var text2 = GameObject.FindGameObjectWithTag("loanText");
        if (text2 != null)
            text2.GetComponent<TMPro.TMP_Text>().text = "Debt: " + loan;
    }
    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateText();

        if (scene.buildIndex == (int)Scenes.Play)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
