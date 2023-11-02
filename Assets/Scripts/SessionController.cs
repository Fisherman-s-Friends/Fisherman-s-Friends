using System;
using UnityEngine;
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

        UpdateMoneyText();
    }

    void Update()
    {
            
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
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

        UpdateMoneyText();
        UpdateDayCount();
    }
    public void UpdateDayCount()
    {
        DayCount++;
    }

    private void UpdateMoneyText()
    {
        var text = GameObject.FindGameObjectWithTag("moneyText");
        if (text != null)
            text.GetComponent<TMPro.TMP_Text>().text = money + " fishes";
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 75, 50), "Add money"))
        {
            AddMoney(10);
        }
        if (GUI.Button(new Rect(100, 10, 75, 50), "Next day"))
        {
            NextDay();
        }
    }
}
