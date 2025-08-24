using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    
    public double manaPoints = 0;
    public double idleClicks = 0;
    public double clickValue = 1;

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
}