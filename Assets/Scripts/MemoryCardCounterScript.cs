using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryCardCounterScript: MonoBehaviour
{
    [Header("Cards data")]
    [SerializeField] private List<ScriptableCard> cards = new List<ScriptableCard>();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI counterText;

    private void Start()
    {
        RefreshCounter();
    }

    private void OnEnable()
    {
        RefreshCounter();
    }

    public void RefreshCounter()
    {
        if (counterText == null) return;

        int unlockedCount = 0;
        int totalCount = 0;

        if (cards != null)
        {
            totalCount = cards.Count;

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null && cards[i].isUnlocked)
                {
                    unlockedCount++;
                }
            }
        }

        counterText.text = $"{unlockedCount}/{totalCount}";
    }
}