using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)

    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        // turning off drop
        if (rb2D != null)
        {
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, Input.mousePosition, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.name == "Drop")
        {
            image.raycastTarget = true;
            transform.SetParent(transform.root);
            if (rb2D != null)
            {
                rb2D.bodyType = RigidbodyType2D.Dynamic;
                rb2D.WakeUp();
            }
            Debug.Log("Item zrzucony na 'Drop' i spada");
        }
        else
        {
            ReturnToInventory();
        }
    }

    private void ReturnToInventory()
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        if (rb2D != null)
        {
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
            rb2D.Sleep();
        }
        Debug.Log("Item wraca do slotu.");
    }
    private void MoveItemToSlot(Transform slot)
    {
        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;

        if (rb2D != null)
        {
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
            rb2D.Sleep();
        }
        Debug.Log($"Item został przeniesiony do slota: {slot.name}");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Back"))
        {
            Debug.Log("Item dotknął podłogi");
            ReturnToInventory();
        }
        else if (collision.gameObject.CompareTag("Pot"))
        {
            Debug.Log("Item wpadł do kotła");
            Transform potSlot = GameObject.Find("potSlot").transform;
            Transform potSlot2 = GameObject.Find("potSlot2").transform;
            if (potSlot != null && potSlot.childCount == 0)
            {
                 MoveItemToSlot(potSlot);
            }
            else if(potSlot2 != null && potSlot2.childCount == 0)
            {
                MoveItemToSlot(potSlot2);
            }
            else
            {
                ReturnToInventory();
                Debug.LogWarning("Nie znaleziono PotSlota w Pot!");
            }
        }
    }
}

