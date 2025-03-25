using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0){
        GameObject dropped = eventData.pointerDrag;
        DragableItem dragableItem = dropped.GetComponent<DragableItem>();
        dragableItem.parentAfterDrag = transform;
        }

    }
}
