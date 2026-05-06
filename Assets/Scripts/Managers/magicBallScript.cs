using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class magicBallScript : MonoBehaviour
{
    public Text pointDisplay;
    public Text CPSDisplay;
    public List<UpgradeEntry> upgrades = new List<UpgradeEntry>();


    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        pointDisplay.text = FormatBigNumberEnglish(GameManager.Instance.manaPoints);
        CPSDisplay.text = "CPS: " + Convert.ToInt32(GameManager.Instance.GetEffectiveIdle());

        if (timer >= 1f)
        {
            GameManager.Instance.manaPoints += GameManager.Instance.GetEffectiveIdle();
            timer -= 1f;
        }

    }
    public double GetCurrentMana()
    {
        return GameManager.Instance.manaPoints;
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
    public string FormatBigNumberEnglish2(double number)
    {
        if (number >= 1_000_000_000)
            return (number / 1_000_000_000d).ToString("0.#") + "B";
        else if (number >= 1_000_000)
            return (number / 1_000_000d).ToString("0.#") + "M";
        else if (number >= 100_000)
            return (number / 1_000d).ToString("0.#") + "K";
        else
            return number.ToString("0");
    }
    public float GetPriceMultiplier()
    {
        return Mathf.Log10((float)GameManager.Instance.idleClicks + 10);
    }



    public bool SpendMana(double amount)
    {
        if (GameManager.Instance.manaPoints >= amount)
        {
            GameManager.Instance.manaPoints -= amount;
            return true;
        }
        return false;
    }

    public void BallClicked()
    {
        GameManager.Instance.manaPoints += GameManager.Instance.GetEffectiveClick();

        ;
    }
    [Serializable]
    public enum UpgradeType
    {
        ClickPower,
        IdleGain
    }
    [Serializable]
    public class UpgradeEntry //data for upgrades in store
    {
        public UpgradeType type;
        public double cost = 50;
        public float effectAmount = 1f;
        public Text priceText;

        public Text descText;

        public GameObject upgradeImage;

        public GameObject blockedImage;
        public bool isPurchased = false;
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
                    GameManager.Instance.clickValue += u.effectAmount;
                    u.isPurchased = true;
                    break;
                case UpgradeType.IdleGain:
                    GameManager.Instance.idleClicks += u.effectAmount;
                    u.isPurchased = true;
                    break;
            }
            if (u.isPurchased == true)
            {
                u.blockedImage.SetActive(false);
            }
            u.cost *= 1.15;
            if (u.isPurchased == true)
            {
                u.upgradeImage.SetActive(true);
            }
            if (u.priceText != null)
                u.priceText.text = Convert.ToInt32(u.cost).ToString();
            u.priceText.text = FormatBigNumberEnglish2(u.cost);
        }
        else
        {
            Debug.Log("Za mało many.");
        }
    }


}
