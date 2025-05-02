using System;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    [SerializeField] private RectTransform canvasRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransorm;
    private RectTransform rectTransform;
    private void Awake()
    {
        instance = this;
        backgroundRectTransorm = transform.Find("Background").GetComponent<RectTransform>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        SetText("Hejka");
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransorm.sizeDelta = textSize + paddingSize;
    }
    private void Update()
    {
        Vector3 paddingSize = new Vector2(2, 2);

        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x + paddingSize;

        if (anchoredPosition.x + backgroundRectTransorm.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransorm.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransorm.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransorm.rect.height;
        }


        rectTransform.anchoredPosition = anchoredPosition;
    }

    
}
