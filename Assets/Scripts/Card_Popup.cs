using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Memory_Card_Popup : MonoBehaviour
{
    [Header("Popup panel (child)")]
    [Tooltip("Child GameObject z UI (np. panel), który ma być pokazywany/ukrywany.")]
    public GameObject popupRoot;        // <- tutaj child z panelem

    [Header("Card")]
    [Tooltip("Image, na którym wyświetlana jest grafika karty (najczęściej child 'Card').")]
    public Image cardImage;             // Image na obiekcie 'Card'

    [Header("Frames (children of Card)")]
    [Tooltip("Ramka pionowa (Frame1).")]
    public GameObject framePortrait;    // 'Frame1'

    [Tooltip("Ramka pozioma (Frame2).")]
    public GameObject frameLandscape;   // 'Frame2'

    [Header("Timing")]
    [Tooltip("Jak długo popup ma być widoczny.")]
    public float displayTime = 8f;

    private Coroutine currentRoutine;

    private void Start()
    {
        // Na starcie chowamy sam panel (child), parent ze skryptem ZOSTAJE aktywny
        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
        }
    }

    public void ShowCard(ScriptableCard card)
    {
        if (card == null)
        {
            Debug.LogWarning("Memory_Card_Popup.ShowCard called with null card.", this);
            return;
        }

        if (cardImage == null)
        {
            Debug.LogWarning("Memory_Card_Popup: cardImage nie jest ustawione.", this);
            return;
        }

        // Ustaw sprite karty
        cardImage.sprite = card.artwork;
        cardImage.preserveAspect = true;

        // Wybierz ramkę na podstawie orientacji
        if (framePortrait != null)
            framePortrait.SetActive(card.orientation == CardOrientation.Portrait);

        if (frameLandscape != null)
            frameLandscape.SetActive(card.orientation == CardOrientation.Landscape);

        // Włącz panel
        if (popupRoot != null)
        {
            popupRoot.SetActive(true);
        }

        // Odpal timer chowania
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
        }

        currentRoutine = null;
    }
}