using System.Collections.Generic;
using UnityEngine;


public class ShopManager : MonoBehaviour
{
    [Header("Itemy do losowania")]
    public List<ScriptableItem> possibleItems;

    [Header("Prefab przedmiotu w sklepie")]
    public GameObject itemPrefab;

    [Header("Rodzic dla itemów w sklepie")]
    public Transform shopParent;

    [Header("Sloty w sklepie")]
    public List<Transform> shopSlots;

    public magicBallScript manaManager;
    public Transform inventoryParent;
    void Start()
    {
        while (FindEmptySlot() != null)
            SpawnRandomItem();
    }
    public void SpawnRandomItem()
    {
        if (possibleItems == null || possibleItems.Count == 0)
        {
            Debug.LogWarning("Brak itemów do losowania");
            return;
        }

        ScriptableItem randomItem = possibleItems[Random.Range(0, possibleItems.Count)];

        Transform emptySlot = FindEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("Brak wolnych slotów w sklepie");
            return;
        }

        GameObject newItem = Instantiate(itemPrefab, emptySlot);
        newItem.transform.localPosition = Vector3.zero;

        DragableItem dragable = newItem.GetComponent<DragableItem>();
        if (dragable != null)
        {
            dragable.InitialiseItem(randomItem);
            dragable.inShop = true;
        }
        BuyFunction clickHandler = newItem.AddComponent<BuyFunction>();
        clickHandler.Initialise(randomItem, manaManager, inventoryParent, itemPrefab);
    }

    public Transform FindEmptySlot()
    {
        foreach (Transform slot in shopSlots)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
}
