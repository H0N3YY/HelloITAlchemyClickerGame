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

    // public void AttachToSlot(RectTransform slotRectTransform)
    // {
    //     RectTransform tooltipRectTransform = GetComponent<RectTransform>();

    //     Vector3[] slotCorners = new Vector3[4];
    //     slotRectTransform.GetWorldCorners(slotCorners);

    //     Vector3 topCenter = (slotCorners[1] + slotCorners[2]) / 2;
    //     Vector2 anchoredPos;
    // RectTransform canvasRect = tooltipRectTransform.root as RectTransform;
    
    // if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //     canvasRect,
    //     RectTransformUtility.WorldToScreenPoint(null, topCenter),
    //     null,
    //     out anchoredPos))
    // {
    //     tooltipRectTransform.anchoredPosition = anchoredPos + new Vector2(0, 10f);
    // }
    // }


}
