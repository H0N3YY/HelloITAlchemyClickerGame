using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI valueText;
    public Image countBackground;
    public ScriptableItem item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Tooltip tooltip;
    [HideInInspector] public Transform parentAfterDrag;
    private Rigidbody2D rb2D;
    private BoxCollider2D bc2d;
    private bool dragState;
    [HideInInspector] public bool inShop = false;

    private Coroutine holdCoroutine;
    private float requiredHoldTime = 1f;
    [SerializeField] public Sprite newSlotSprite;
    [SerializeField] private Sprite previousSprite;

    private void Start()
    {
        tooltip = tooltip = Object.FindFirstObjectByType<Tooltip>(FindObjectsInactive.Include);
        InitialiseItem(item);
    }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }
    public void InitialiseItem(ScriptableItem newItem)
    {
        image.sprite = newItem.image;
        if (valueText != null)
        valueText.text = newItem.sellValue.ToString();
        RefreshCount();
    }
    public void RefreshCount()
    {
        if (countText != null)
            countText.text = count.ToString();
        bool textActive = count > 1;
        if (countBackground != null)
            countBackground.gameObject.SetActive(textActive);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inShop) return;
        if (dragState == false)
        {
            holdCoroutine = StartCoroutine(HoldTimer());
            Debug.Log("Kursor najechał na item");
        }
    }
public void OnPointerExit(PointerEventData eventData)
{
    if (inShop) return;

    if (holdCoroutine != null)
    {
        StopCoroutine(holdCoroutine);
        holdCoroutine = null;
    }

    // jeśli kursor nad tooltipem, nie chowaj
    if (eventData.pointerCurrentRaycast.gameObject != null &&
        eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Tooltip>() != null)
    {
        Debug.Log("Kursor opuścił item, ale wszedł na tooltip");
        return;
    }

    Tooltip.Instance.HideIfNeeded();
    ImageCleaner();
    Debug.Log("Kursor opuścił item");
}



    public void OnBeginDrag(PointerEventData eventData)

    {
        if (inShop) return;
        dragState = true;
        bc2d.enabled = false;
        ImageCleaner();
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
        if (inShop) return;
        dragState = true;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        Vector3 globalMousePos;
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, Input.mousePosition, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inShop) return;
        // Drop
        if (eventData.pointerEnter != null && eventData.pointerEnter.name == "Drop")
        {
            bc2d.enabled = true;
            image.raycastTarget = true;
            transform.SetParent(transform.root);
            if (rb2D != null)
            {
                rb2D.bodyType = RigidbodyType2D.Dynamic;
                rb2D.WakeUp();
                ImageCleaner();
            }
        }
        // Stack
        else if (parentAfterDrag != null)
        {
            DragableItem existingItem = parentAfterDrag.GetComponentInChildren<DragableItem>();

            if (existingItem != null && existingItem != this)
            {
                if (existingItem.item != this.item)
                {
                    Debug.Log("Nie można połączyć z innym typem itemu");
                    ReturnToInventory();
                    return;
                }


                existingItem.count += this.count;
                existingItem.RefreshCount();
                Destroy(gameObject);
                return;

            }

            else
            {
                ReturnToInventory();
            }
        }
    }

    public void ReturnToInventory()
    {
        dragState = false;
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        if (rb2D != null)
        {
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
            rb2D.Sleep();
        }
        Debug.Log("Item wraca do slotu");
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
                if (count > 1)
                {
                    count--;
                    RefreshCount();
                    GameObject itemCopy = Instantiate(gameObject, transform.position, Quaternion.identity);
                    DragableItem copy = itemCopy.GetComponent<DragableItem>();
                    copy.count = 1;
                    RefreshCount();
                    ReturnToInventory();

                    copy.MoveItemToSlot(potSlot);
                    copy.bc2d.enabled = false;
                }
                else if (count == 1)
                {
                    MoveItemToSlot(potSlot);
                    bc2d.enabled = false;
                }
                else
                {
                    ReturnToInventory();
                }

            }
            else if (potSlot2 != null && potSlot2.childCount == 0)
            {
                if (count > 1)
                {
                    count--;
                    RefreshCount();
                    GameObject itemCopy = Instantiate(gameObject, transform.position, Quaternion.identity);
                    DragableItem copy = itemCopy.GetComponent<DragableItem>();
                    copy.count = 1;
                    RefreshCount();
                    ReturnToInventory();

                    copy.MoveItemToSlot(potSlot2);
                    copy.bc2d.enabled = false;
                }
                else if (count == 1)
                {
                    MoveItemToSlot(potSlot2);
                    bc2d.enabled = false;
                }
                else
                    ReturnToInventory();
            }
            else
                ReturnToInventory();
        }

        else if (collision.gameObject.CompareTag("Plant"))
        {
            Debug.Log("Item dotknął roślinki");

            PlantGrowth plant = collision.gameObject.GetComponent<PlantGrowth>();
            if (plant != null)
            {
                plant.SetPlantedItem(this.item);

                if (plant.plantVisualsRenderer != null)
                {
                    plant.plantVisualsRenderer.gameObject.SetActive(true);
                }

                plant.StartGrowth();

                if (count > 1)
                {
                    count--;
                    RefreshCount();


                    ReturnToInventory();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("Nie znaleziono skryptu PlantGrowth na obiekcie z tagiem Plant");
            }
        }

    }
    private IEnumerator HoldTimer()
    {
        yield return new WaitForSeconds(requiredHoldTime);
        TriggerAction();
        transform.parent.GetComponent<Image>().sprite = newSlotSprite;
    }
    private void TriggerAction()
    {
        Tooltip.Instance.AttachToSlot(GetComponent<RectTransform>());
        Tooltip.Instance.ShowForItem(this);
        tooltip.gameObject.SetActive(true);
        Tooltip.Instance.SetText(item.itemName);
        Tooltip.Instance.Setdescription(item.description);
        Debug.Log($"{requiredHoldTime} sekund mineło");

    }
    public void ImageCleaner()
    {
        tooltip.gameObject.SetActive(false);
        tooltip.transform.position = new Vector3(1200, 1200, 0);
        transform.parent.GetComponent<Image>().sprite = previousSprite;
    }
}

