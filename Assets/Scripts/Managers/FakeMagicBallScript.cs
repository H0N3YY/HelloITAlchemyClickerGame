using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class fakemagicBallScript : MonoBehaviour
{
    public Text pointDisplay;
    public Text CPSDisplay;
    public List<UpgradeEntry> upgrades = new List<UpgradeEntry>(); 
    
    private double idleClicks = 0;
    private double clickValue = 1;
    private double updatedValue = 0;
    public float priceMultiplier = 1f;

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

    public float GetPriceMultiplier()
    {
        return Mathf.Log10((float)idleClicks + 10); 
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
    }

    [System.Serializable]
    public enum UpgradeType
    {
        ClickPower,
        IdleGain
    }

    [System.Serializable]
    public class UpgradeEntry // data for upgrades in store
    {
        public UpgradeType type;
        public double cost = 50;
        public float effectAmount = 1f;
        public Text priceText;
    }

    public void ApplyUpgrade(int index)
    {
        if (index < 0 || index >= upgrades.Count) return;

        var u = upgrades[index];

        if (SpendMana(u.cost))
        {
            switch (u.type)
            {
                case UpgradeType.ClickPower:
                    clickValue += u.effectAmount;
                    break;
                case UpgradeType.IdleGain:
                    idleClicks += u.effectAmount;
                    break;
            }

            u.cost *= 1.15;
            if (u.priceText != null)
                u.priceText.text = Convert.ToInt32(u.cost).ToString();
        }
        else
        {
            Debug.Log("Za mało many.");
        }
    }
}
