using System;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    public static Tooltip Instance => instance;
    [SerializeField] private Canvas canvas; 


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private void Awake()
    {
        instance = this;
        rectTransform = transform.GetComponent<RectTransform>();

        SetText("Item Name");
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

    }
    // Tooltip to mouse Script
    // private void Update()
    // {
    //     Vector3 paddingSize = new Vector2(2, 2);

    //     Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x + paddingSize;

    //     if (anchoredPosition.x + backgroundRectTransorm.rect.width > canvasRectTransform.rect.width)
    //     {
    //         anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransorm.rect.width;
    //     }
    //     if (anchoredPosition.y + backgroundRectTransorm.rect.height > canvasRectTransform.rect.height)
    //     {
    //         anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransorm.rect.height;
    //     }


    //     rectTransform.anchoredPosition = anchoredPosition;
    // }


    // Tooltip to slot Script
    public void AttachToSlot(RectTransform slotRectTransform)
    {
        Vector3[] slotCorners = new Vector3[4];
        slotRectTransform.GetWorldCorners(slotCorners);

        Vector3 topCenter = (slotCorners[1] + slotCorners[2]) / 2;

        Vector2 anchoredPos;
        Camera cam = canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(cam, topCenter),
            cam,
            out anchoredPos))
        {
            // Dodajemy offset
            anchoredPos += new Vector2(-450, -5f);

            // --- GRANICE ---
            float tooltipWidth = rectTransform.rect.width;
            float tooltipHeight = rectTransform.rect.height;

            float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
            float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

            // Ogranicz X
            if (anchoredPos.x + tooltipWidth > canvasWidth / 2)
            {
                anchoredPos.x = canvasWidth / 2 - tooltipWidth;
            }
            if (anchoredPos.x - tooltipWidth < -canvasWidth / 2)
            {
                anchoredPos.x = -canvasWidth / 2 + tooltipWidth;
            }

            // Ogranicz Y
            if (anchoredPos.y + tooltipHeight > canvasHeight / 2)
            {
                anchoredPos.y = canvasHeight / 2 - tooltipHeight;
            }
            if (anchoredPos.y - tooltipHeight < -canvasHeight / 2)
            {
                anchoredPos.y = -canvasHeight / 2 + tooltipHeight;
            }

            rectTransform.anchoredPosition = anchoredPos;
        }
    }



}
