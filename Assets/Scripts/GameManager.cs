using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI moneyTxt;
    public TextMeshProUGUI upgradeTxt;
    public int point;
    public float decreaseDuration = 10f;
    private const string NotesCollectedKey = "NotesCollected";

    public GameObject roomNum3;
    public GameObject roomNum3UIBox;

    public bool upgraded;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        point = PlayerPrefs.GetInt(NotesCollectedKey, point);
        UpdateMoneyUI();
    }
    private void Update()
    {
        UpdateMoneyUI();
    }
    public void AddPoint()
    {
        point += 100;
        upgraded = true;
        moneyTxt.text = "= " + point.ToString();
        PlayerPrefs.SetInt(NotesCollectedKey, point);
    }
    
    void UpdateMoneyUI()
    {
        moneyTxt.text = "= " + point.ToString();
        
    }

    IEnumerator DecreaseMoneySmoothly(int amount)
    {
        int targetMoney = Mathf.Max(point - amount, 0);
        float startTime = Time.time;

        while (Time.time - startTime < decreaseDuration)
        {
            float t = (Time.time - startTime) / decreaseDuration;
            point = (int)Mathf.Lerp(point, targetMoney, t);
            UpdateMoneyUI();
            yield return null;

        }
        
        point = targetMoney;
        UpdateMoneyUI();
        upgradeTxt.text = "UPGRADE FOR = \r\n" + point.ToString();


    }
    IEnumerator UpgradingTime()
    {
        yield return new WaitForSeconds(1);
        roomNum3.SetActive(true);
        roomNum3UIBox.SetActive(false);
    }
    public void MinusPoints(int amount = 800)
    {
        if (upgraded)
        {
            StartCoroutine(DecreaseMoneySmoothly(amount));
            StartCoroutine(UpgradingTime());
        }
        

    }
}