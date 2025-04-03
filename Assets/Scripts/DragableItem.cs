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
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.name == "Drop")
        {
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
        Debug.Log("pointerEnter: " + (eventData.pointerEnter != null ? eventData.pointerEnter.name : "NULL"));
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Back"))
        {
            Debug.Log("Item dotknął podłogi");
            ReturnToInventory();
        }
    }
}
