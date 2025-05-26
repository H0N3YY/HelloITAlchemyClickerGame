using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DragableItem droppedItem = dropped.GetComponent<DragableItem>();
        if (droppedItem == null) return;

        
        DragableItem existingItem = GetComponentInChildren<DragableItem>();

        // SLOT PUSTY 
        if (existingItem == null)
        {
            droppedItem.parentAfterDrag = transform;
        }
        // SLOT ZAWIERA TEN SAM ITEM 
        else if (existingItem.item == droppedItem.item)
        {
            droppedItem.parentAfterDrag = transform;
        }
        // SLOT ZAWIERA INNY ITEM 
        else
        {
            Debug.Log("Nie można połączyć różnych itemów.");
            droppedItem.ReturnToInventory();
        }
    }
}
