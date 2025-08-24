using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public double manaPoints = 0;
    public double idleClicks = 0;
    public double clickValue = 1;


    public double idleMultiplier = 1.0;
    public double clickMultiplier = 1.0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public double GetEffectiveIdle() => idleClicks * idleMultiplier;
    public double GetEffectiveClick() => clickValue * clickMultiplier;
}