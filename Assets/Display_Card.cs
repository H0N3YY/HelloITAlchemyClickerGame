using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Display_Card : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject cardPopup;
    [SerializeField] private Image artworkImage;
    [SerializeField] private float showDuration = 5f;

    private Coroutine co;

    private void Awake()
    {
        if (cardPopup) cardPopup.SetActive(false);
    }
   public void ShowOnce(ScriptableCard card)
    {
        if (!card || !cardPopup || !artworkImage) return;

        artworkImage.sprite = card.artwork;
        cardPopup.SetActive(true);

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(HideAfter(showDuration));
    }

    private IEnumerator HideAfter(float t)
    {
        yield return new WaitForSeconds(t);
        if (cardPopup) cardPopup.SetActive(false);
        co = null;
    }
}
