using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryCardGallery : MonoBehaviour
{
    [Header("Cards data")]
    [Tooltip("Lista wszystkich ScriptableCard w kolejności, w jakiej mają się wyświetlać.")]
    public List<ScriptableCard> cards = new List<ScriptableCard>();

    [Header("UI references - double (2x portrait)")]
    public GameObject doubleRoot;

    public Image leftCardImage;
    public Image rightCardImage;
    public GameObject panoramicRoot;

    [Tooltip("Image karty panoramicznej.")]
    public Image panoramicCardImage;

    [Header("Locked frame")]
    public Sprite lockedFrameSprite;

    public TextMeshProUGUI pageText;

    // lista indeksów początkowych dla każdej „strony”
    private readonly List<int> pageStartIndices = new List<int>();
    private int currentPage = 0; // 0-based

    private void Start()
    {
        RebuildPages();
        RefreshPage();
    }

    public void RebuildPages()
    {
        pageStartIndices.Clear();

        if (cards == null || cards.Count == 0)
        {
            pageStartIndices.Add(0);
            return;
        }

        int i = 0;
        while (i < cards.Count)
        {
            pageStartIndices.Add(i);

            ScriptableCard c = cards[i];

            if (c != null && c.orientation == CardOrientation.Landscape)
            {

                i += 1;
            }
            else
            {

                i += 2;
            }
        }

        currentPage = Mathf.Clamp(currentPage, 0, pageStartIndices.Count - 1);
    }

    public void NextPage()
    {
        int totalPages = GetTotalPages();
        if (totalPages <= 0) return;

        currentPage++;
        if (currentPage >= totalPages)
            currentPage = 0; // pętla

        RefreshPage();
    }

    public void PrevPage()
    {
        int totalPages = GetTotalPages();
        if (totalPages <= 0) return;

        currentPage--;
        if (currentPage < 0)
            currentPage = totalPages - 1; // pętla

        RefreshPage();
    }

    private int GetTotalPages()
    {
        return Mathf.Max(1, pageStartIndices.Count);
    }

    private void RefreshPage()
    {
        int totalPages = GetTotalPages();
        currentPage = Mathf.Clamp(currentPage, 0, totalPages - 1);

        if (pageStartIndices.Count == 0)
        {
            ToggleDouble(false);
            TogglePanoramic(false);
            if (pageText != null) pageText.text = "0/0";
            return;
        }

        int startIndex = pageStartIndices[currentPage];
        ScriptableCard firstCard =
            (cards != null && startIndex < cards.Count) ? cards[startIndex] : null;


        if (pageText != null && cards != null && cards.Count > 0)
        {
            pageText.text = $"{startIndex + 1}/{cards.Count}";
        }

        bool isPanoramic =
            firstCard != null && firstCard.orientation == CardOrientation.Landscape;

        if (isPanoramic)
        {

            ToggleDouble(false);
            TogglePanoramic(true);

            SetupSlot(panoramicCardImage, startIndex);
        }
        else
        {

            TogglePanoramic(false);
            ToggleDouble(true);

            SetupSlot(leftCardImage, startIndex);
            SetupSlot(rightCardImage, startIndex + 1);
        }
    }

    private void SetupSlot(Image slotImage, int cardIndex)
    {
        if (slotImage == null) return;

        if (cards == null || cardIndex >= cards.Count)
        {
            slotImage.gameObject.SetActive(false);
            return;
        }
        slotImage.gameObject.SetActive(true);

        ScriptableCard card = cards[cardIndex];
        bool unlockedAndHasArt =
            card != null && card.isUnlocked && card.artwork != null;

        if (unlockedAndHasArt)
        {
            slotImage.sprite = card.artwork;
        }
        else
        {
            slotImage.sprite = lockedFrameSprite;
        }

        slotImage.preserveAspect = true;
    }

    private void ToggleDouble(bool active)
    {
        if (doubleRoot != null)
            doubleRoot.SetActive(active);
    }

    private void TogglePanoramic(bool active)
    {
        if (panoramicRoot != null)
            panoramicRoot.SetActive(active);
    }
}