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

    [Header("UI references - panoramic")]
    public GameObject panoramicRoot;

    [Tooltip("Image karty panoramicznej.")]
    public Image panoramicCardImage;

    [Header("UI references - single (1x portrait)")]
    public GameObject SingleRoot;
    public Image singleCardImage;

    [Header("Locked frame")]
    public Sprite lockedFrameSprite;

    public TextMeshProUGUI pageText;

    private readonly List<PageData> pages = new List<PageData>();
    private int currentPage = 0;

    private struct PageData
    {
        public int leftIndex;
        public int rightIndex;
        public bool isPanoramic;
        public bool isSingle;

        public PageData(int leftIndex, int rightIndex, bool isPanoramic, bool isSingle)
        {
            this.leftIndex = leftIndex;
            this.rightIndex = rightIndex;
            this.isPanoramic = isPanoramic;
            this.isSingle = isSingle;
        }
    }

    private void Start()
    {
        RebuildPages();
        RefreshPage();
    }

    public void RebuildPages()
    {
        pages.Clear();

        if (cards == null || cards.Count == 0)
        {
            pages.Add(new PageData(-1, -1, false, false));
            return;
        }

        int i = 0;

        while (i < cards.Count)
        {
            ScriptableCard currentCard = cards[i];

            bool currentIsLandscape =
                currentCard != null &&
                currentCard.orientation == CardOrientation.Landscape;

            if (currentIsLandscape)
            {
                // Landscape zawsze dostaje osobną stronę panoramiczną
                pages.Add(new PageData(i, -1, true, false));
                i++;
                continue;
            }

            if (i + 1 < cards.Count)
            {
                ScriptableCard nextCard = cards[i + 1];

                bool nextIsPortrait =
                    nextCard != null &&
                    nextCard.orientation != CardOrientation.Landscape;

                if (nextIsPortrait)
                {
                    // Dwie karty Portrait na jednej stronie Double
                    pages.Add(new PageData(i, i + 1, false, false));
                    i += 2;
                }
                else
                {
                    // Aktualna karta Portrait sama, bo następna jest Landscape
                    pages.Add(new PageData(i, -1, false, true));
                    i++;
                }
            }
            else
            {
                // Ostatnia karta Portrait sama
                pages.Add(new PageData(i, -1, false, true));
                i++;
            }
        }

        currentPage = Mathf.Clamp(currentPage, 0, pages.Count - 1);
    }

    public void NextPage()
    {
        int totalPages = GetTotalPages();
        if (totalPages <= 0) return;

        currentPage++;

        if (currentPage >= totalPages)
            currentPage = 0;

        RefreshPage();
    }

    public void PrevPage()
    {
        int totalPages = GetTotalPages();
        if (totalPages <= 0) return;

        currentPage--;

        if (currentPage < 0)
            currentPage = totalPages - 1;

        RefreshPage();
    }

    private int GetTotalPages()
    {
        return Mathf.Max(1, pages.Count);
    }

    private void RefreshPage()
    {
        int totalPages = GetTotalPages();
        currentPage = Mathf.Clamp(currentPage, 0, totalPages - 1);

        if (pages.Count == 0)
        {
            ToggleDouble(false);
            TogglePanoramic(false);
            ToggleSingle(false);
            UpdatePageText();
            return;
        }

        PageData page = pages[currentPage];

        if (page.isPanoramic)
        {
            ToggleDouble(false);
            ToggleSingle(false);
            TogglePanoramic(true);

            SetupSlot(panoramicCardImage, page.leftIndex);
        }
        else if (page.isSingle)
        {
            ToggleDouble(false);
            TogglePanoramic(false);
            ToggleSingle(true);

            SetupSlot(singleCardImage, page.leftIndex);
        }
        else
        {
            TogglePanoramic(false);
            ToggleSingle(false);
            ToggleDouble(true);

            SetupSlot(leftCardImage, page.leftIndex);
            SetupSlot(rightCardImage, page.rightIndex);
        }

        UpdatePageText();
    }

    private void SetupSlot(Image slotImage, int cardIndex)
    {
        if (slotImage == null) return;

        if (cards == null || cardIndex < 0 || cardIndex >= cards.Count)
        {
            slotImage.gameObject.SetActive(false);
            return;
        }

        slotImage.gameObject.SetActive(true);

        ScriptableCard card = cards[cardIndex];

        bool unlockedAndHasArt =
            card != null &&
            card.isUnlocked &&
            card.artwork != null;

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

    private void UpdatePageText()
    {
        if (pageText == null) return;

        pageText.text = $"{currentPage + 1}/{GetTotalPages()}";
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

    private void ToggleSingle(bool active)
    {
        if (SingleRoot != null)
            SingleRoot.SetActive(active);
    }
}