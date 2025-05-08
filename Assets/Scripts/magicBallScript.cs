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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
    {
        timer += Time.deltaTime;
        pointDisplay.text = Convert.ToInt32(updatedValue).ToString();
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
    public void UpgradeOne()
    {
        if (updatedValue >= costOne)
        {
            clickValue += 1;
            updatedValue -= costOne;
            costOne = costOne* 1.15;
            priceDisplay1.text = Convert.ToInt32(costOne).ToString();


        }
        else
        {
            clickValue = clickValue;
            
        }
      

    }
    public void UpgradeTwo()
    {
        if (updatedValue >= costTwo)
        {
            idleClicks += 10;
            updatedValue -= costTwo;
            costTwo = costTwo * 1.15;
            priceDisplay2.text = Convert.ToInt32(costTwo).ToString();


        }
        else
        {
            clickValue = clickValue;

        }


    }

}
