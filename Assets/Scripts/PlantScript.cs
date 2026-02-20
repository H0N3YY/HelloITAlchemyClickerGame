using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrowth : MonoBehaviour
{
    [Header("Growth Stages Sprites")]
    public List<Sprite> growthStages;

    [Header("Plant Visuals (UI Image)")]
    [Tooltip("UI Image used to display plant stages (enable Preserve Aspect on this).")]
    public Image plantVisualsImage;

    [Header("Plant Visuals (SpriteRenderer) - optional")]
    [Tooltip("Optional SpriteRenderer for world-space plants.")]
    public SpriteRenderer plantVisualsRenderer;

    public GameObject draggableItemPrefab;

    [Header("Item Spawn Point")]
    public Transform spawnPoint;

    [Header("Growth Interval")]
    public float growthInterval = 20f;

    [Header("Equipment")]
    public Transform inventoryParent;

    // Optional components on the same GameObject (fallbacks)
    private Image plantImage;
    private SpriteRenderer plantRenderer;

    private ScriptableItem plantedItem;

    private int currentStage = 0;
    private bool isReadyToHarvest = false;
    private Coroutine growthCoroutine;

    private void Awake()
    {
        // Fallback components on the same GameObject
        plantImage = GetComponent<Image>();
        plantRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanPlant(ScriptableItem item)
    {
        if (item == null)
            return false;

        if (item.actionType != ActionType.Plant)
            return false;

        if (item.type != ItemType.Seed)
            return false;

        return true;
    }

    public void SetPlantedItem(ScriptableItem item)
    {
        // Validate item before planting
        if (!CanPlant(item))
        {
            if (item == null)
            {
                Debug.LogWarning("SetPlantedItem called with null item. Planting aborted.");
            }
            else
            {
                Debug.LogWarning($"Item '{item.itemName}' cannot be planted. " +
                                 $"Required: type = Seed, actionType = Plant. " +
                                 $"Got: type = {item.type}, actionType = {item.actionType}");
            }
            return;
        }

        // Remember which seed was planted
        plantedItem = item;

        // Make sure the list exists
        if (growthStages == null)
            growthStages = new List<Sprite>();
        else
            growthStages.Clear();

        // Fill growth stages from ScriptableItem
        if (item.plantStage1 != null)
            growthStages.Add(item.plantStage1);

        if (item.PlantStage2 != null)
            growthStages.Add(item.PlantStage2);

        if (item.PlantStage3 != null)
            growthStages.Add(item.PlantStage3);

        if (growthStages.Count == 0)
        {
            Debug.LogWarning($"Seed {item.itemName} has no plant stage sprites assigned!");
        }
    }
    public void StartGrowth()
    {
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
        }

        isReadyToHarvest = false;
        currentStage = 0;

        // Make sure visuals are visible
        if (plantVisualsImage != null && !plantVisualsImage.gameObject.activeSelf)
        {
            plantVisualsImage.gameObject.SetActive(true);
        }
        else if (plantVisualsRenderer != null && !plantVisualsRenderer.gameObject.activeSelf)
        {
            plantVisualsRenderer.gameObject.SetActive(true);
        }

        // Safety check: make sure we actually have some growth stages
        if (growthStages == null || growthStages.Count == 0)
        {
            Debug.LogWarning("Growth started, but growthStages list is empty. Did you call SetPlantedItem?");
            return;
        }

        growthCoroutine = StartCoroutine(GrowthCycle());
    }

    private IEnumerator GrowthCycle()
    {
        while (currentStage < growthStages.Count)
        {
            SetSprite(growthStages[currentStage]);
            currentStage++;

            if (currentStage >= growthStages.Count)
            {
                break;
            }

            yield return new WaitForSeconds(growthInterval);
        }

        isReadyToHarvest = true;
        Debug.Log("Roślinka jest gotowa do zebrania!");
    }

    private void SetSprite(Sprite newSprite)
    {
        // Preferred: dedicated UI Image
        if (plantVisualsImage != null)
        {
            plantVisualsImage.sprite = newSprite;
            plantVisualsImage.preserveAspect = true; // make sure aspect is preserved
        }
        // Fallback: dedicated SpriteRenderer
        else if (plantVisualsRenderer != null)
        {
            plantVisualsRenderer.sprite = newSprite;
        }
        // Fallback: Image on the same GameObject
        else if (plantImage != null)
        {
            plantImage.sprite = newSprite;
        }
        // Fallback: SpriteRenderer on the same GameObject
        else if (plantRenderer != null)
        {
            plantRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("No visual component found to set sprite on the plant.");
        }

        if (newSprite != null)
        {
            Debug.Log($"Set plant sprite to: {newSprite.name} (stage {currentStage + 1})");
        }
    }

    public void Harvest()
    {
        if (!isReadyToHarvest)
        {
            Debug.Log("Jeszcze nie można zebrać roślinki");
            return;
        }

        Debug.Log("Zbieram roślinkę");

        // Hide plant visuals
        if (plantVisualsImage != null)
            plantVisualsImage.gameObject.SetActive(false);
        else if (plantVisualsRenderer != null)
            plantVisualsRenderer.gameObject.SetActive(false);

        if (plantedItem != null && inventoryParent != null)
        {
            foreach (Transform slot in inventoryParent)
            {
                if (slot.childCount == 0)
                {
                    GameObject newItem = Instantiate(draggableItemPrefab, slot);
                    newItem.transform.localPosition = Vector3.zero;

                    // Ustaw item + count = 2
                    DragableItem di = newItem.GetComponent<DragableItem>();
                    if (di != null)
                    {
                        di.item = plantedItem;
                        di.count = 2;
                        di.InitialiseItem(plantedItem);
                    }

                    Debug.Log("Zebrano roślinkę, dodano item z count = 2");
                    break;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isReadyToHarvest)
        {
            Debug.Log("Jeszcze nie można zebrać tej roślinki");
            return;
        }

        Debug.Log("Kliknięto roślinkę");

        Harvest();
    }
}
