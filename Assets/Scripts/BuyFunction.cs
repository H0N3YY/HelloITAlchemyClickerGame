using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BuyFunction : MonoBehaviour, IPointerClickHandler
{
    private ScriptableItem item;
    private magicBallScript manaManager;
    private Transform inventoryParent;
    private GameObject draggableItemPrefab;
    private TextMeshProUGUI priceText;

    public void Initialise(ScriptableItem newItem, magicBallScript manaSource, Transform inventoryParentTarget, GameObject itemPrefab)
    {
        item = newItem;
        manaManager = manaSource;
        inventoryParent = inventoryParentTarget;
        draggableItemPrefab = itemPrefab;
        Transform price = transform.Find("Price");
        if (price != null)
        {
            priceText = price.GetComponent<TextMeshProUGUI>();
        }

    }
    private void Update()
    {
        if (item != null && manaManager != null && priceText != null)
        {
            int dynamicPrice = Mathf.CeilToInt(item.sellValue * manaManager.GetPriceMultiplier());
            priceText.text = dynamicPrice.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null && manaManager != null)
        {
            if (manaManager.SpendMana(item.sellValue))
            {
                Debug.Log($"Kupiono: {item.itemName} za {item.sellValue} many");

                foreach (Transform slot in inventoryParent)
                {
                    if (slot.childCount == 0)
                    {
                        GameObject newItem = Instantiate(draggableItemPrefab, slot);
                        newItem.transform.localPosition = Vector3.zero;

                        DragableItem di = newItem.GetComponent<DragableItem>();
                        if (di != null)
                        {
                            di.item = item;
                            di.count = 1;
                            Transform priceTag = newItem.transform.Find("Price");
                            if (priceTag != null)
                            {
                                priceTag.gameObject.SetActive(false);
                            }
                            di.inShop = false;
                            di.InitialiseItem(item);

                        }

                        break;
                    }
                }

                Destroy(gameObject); // usuwamy kupiony przedmiot ze sklepu
            }
            else
            {
                Debug.Log("Za mało many!");
            }
        }
    }
}
