using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static Tooltip instance;
    public static Tooltip Instance => instance;


    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private RectTransform backgroundRectTransform;


    [SerializeField] private PotionManager potionManager;
     [SerializeField] private Button useButton;
    private DragableItem currentItem;

    private RectTransform rectTransform;
    private bool isPointerOver = false;
    private void Awake()
    {
        instance = this;
        rectTransform = transform.GetComponent<RectTransform>();

        SetText("Item Name");

        if (useButton) useButton.onClick.RemoveAllListeners();

    }
    

    public void SetText(string tooltipText)
    {
        itemName.SetText(tooltipText);
        itemName.ForceMeshUpdate();
    }
    public void Setdescription(string tooltiplore)
    {
       description.SetText(tooltiplore);
    description.ForceMeshUpdate();
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
     public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        HideIfNeeded();
    }
    public void HideIfNeeded()
    {
        if (!isPointerOver)
        {
            gameObject.SetActive(false);

            if (currentItem != null)
            {
                currentItem.ImageCleaner();
                currentItem = null;
            }
        }
    }
    public void ShowForItem(DragableItem drag)
{
    currentItem = drag;
    gameObject.SetActive(true);

    var si = GetScriptable(drag);

    // Ustaw UI z danych itemu
    if (si != null)
    {
        SetText(si.itemName);
        Setdescription(si.description);
    }

    if (useButton != null)
    {
        useButton.onClick.RemoveAllListeners();

        bool canUse = (si != null && si.effectToTrigger != PotionEffect.None);
        useButton.gameObject.SetActive(canUse);
        useButton.interactable = canUse;

        if (canUse)
        {
            useButton.onClick.AddListener(() =>
            {
                if (potionManager == null)
                {
                    Debug.LogWarning("Tooltip: PotionManager is not assigned.");
                    return;
                }

                // 1) Użyj itemu (efekt + karta on-first-use)
                potionManager.UseItem(si);

                // 2) Zapamiętaj obiekt slota do usunięcia
                var toDestroy = currentItem;

                // 3) Schowaj tooltip i wyczyść slot (wywoła ImageCleaner)
                isPointerOver = false;
                HideIfNeeded();

                // 4) Usuń obiekt z ekwipunku (jeśli nie obsługujesz stacków)
                if (toDestroy != null)
                    Destroy(toDestroy.gameObject);
            });
        }
    }
    else
    {
        Debug.LogWarning("Tooltip: Use Button reference is missing in Inspector.");
    }
}


private ScriptableItem GetScriptable(DragableItem drag)
{
    return drag != null ? drag.item : null;
}

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
public class DragableItemData : MonoBehaviour
{
    public ScriptableItem scriptableItem;
}