using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;

public class magicBallScript : MonoBehaviour
{
    public Text pointDisplay;
    public Text priceDisplay1;
    public Text priceDisplay2;
    public Text CPSDisplay;
    private double idleClicks = 0;
    private double clickValue = 1;
    private double updatedValue = 0;
    private double costOne = 15;
    private double costTwo = 150;

    private float timer = 0f;
    
     void Update()
    {
        timer += Time.deltaTime;
        pointDisplay.text = FormatBigNumberEnglish(updatedValue);
        CPSDisplay.text = "CPS: " + Convert.ToInt32(idleClicks).ToString();

        if (timer >= 1f)
        {
            updatedValue += idleClicks;
            timer -= 1f;
        }
        
    }
    public double GetCurrentMana()
{
    return updatedValue;
}
public string FormatBigNumberEnglish(double number)
{
    if (number >= 1_000_000_000)
        return (number / 1_000_000_000d).ToString("0.#") + "B";
    else if (number >= 1_000_000)
        return (number / 1_000_000d).ToString("0.#") + "M";
    else if (number >= 1_000)
        return (number / 1_000d).ToString("0.#") + "K";
    else
        return number.ToString("0");
}


    public bool SpendMana(double amount)
{
    if (updatedValue >= amount)
    {
        updatedValue -= amount;
        return true;
    }
    return false;
}

    public void BallClicked()
    {
        updatedValue += clickValue;
       
;    }
   public void UpgradeClickPower()
{
    ApplyUpgrade(UpgradeType.ClickPower, ref costOne, 1f, priceDisplay1);
}

public void UpgradeIdleGain()
{
    ApplyUpgrade(UpgradeType.IdleGain, ref costTwo, 10f, priceDisplay2);
}

    [Serializable]
public enum UpgradeType
{
    ClickPower,
    IdleGain
}

public void ApplyUpgrade(UpgradeType type, ref double cost, float effectAmount, Text priceDisplay)
{
    if (SpendMana(cost))
    {
        switch (type)
        {
            case UpgradeType.ClickPower:
                clickValue += effectAmount;
                break;

            case UpgradeType.IdleGain:
                idleClicks += effectAmount;
                break;
        }

        cost *= 1.15;
        priceDisplay.text = Convert.ToInt32(cost).ToString();
    }
    else
    {
        Debug.Log("Za mało many na ulepszenie");
    }
}


}
