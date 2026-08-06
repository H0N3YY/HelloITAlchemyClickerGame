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
    public float growthInterval = 60f;

    [Header("Equipment")]
    public Transform inventoryParent;

    [Header("Sound")]
    [SerializeField] private SoundManager soundManager;

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

        if (soundManager == null)
        {
            soundManager = FindFirstObjectByType<SoundManager>();
        }
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
        Debug.Log($"[PLANT][SetPlantedItem] Ustawiono plantedItem | Object={gameObject.name}, " +
          $"PlantedItem={plantedItem.itemName}, ID={plantedItem.id}");

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

        if (soundManager != null)
        {
            soundManager.plantingPlantSound();
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

        if (soundManager != null)
        {
            soundManager.pickUpPlantSound();
        }

        // Hide plant visuals
        if (plantVisualsImage != null)
            plantVisualsImage.gameObject.SetActive(false);
        else if (plantVisualsRenderer != null)
            plantVisualsRenderer.gameObject.SetActive(false);

        if (plantedItem != null && inventoryParent != null)
        {
            // wybieramy, co ma wypaść po zebraniu
            ScriptableItem rewardItem = plantedItem.harvestResultItem != null
                ? plantedItem.harvestResultItem
                : plantedItem; // fallback: jak nie ustawisz harvestResultItem, to dalej dropi sam siebie

            int rewardCount = plantedItem.harvestResultCount > 0
                ? plantedItem.harvestResultCount
                : 1;

            if (rewardItem == null)
            {
                Debug.LogWarning($"Plant '{plantedItem.itemName}' has no harvestResultItem set. No item will be spawned.");
                return;
            }

            foreach (Transform slot in inventoryParent)
            {
                if (slot.childCount == 0)
                {
                    GameObject newItem = Instantiate(draggableItemPrefab, slot);
                    newItem.transform.localPosition = Vector3.zero;

                    DragableItem di = newItem.GetComponent<DragableItem>();
                    if (di != null)
                    {
                        di.item = rewardItem;
                        di.count = rewardCount;
                        di.InitialiseItem(rewardItem);
                    }

                    Debug.Log($"Zebrano roślinkę, dodano item '{rewardItem.itemName}' x{rewardCount}");
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
    //\/ --- SAVE/LOAD SUPPORT --- \/
    public ScriptableItem PlantedItem => plantedItem;
    public int CurrentStage => currentStage;
    public bool IsReadyToHarvest => isReadyToHarvest;
    public bool IsGrowing => growthCoroutine != null;

    public void LoadPlantState(ScriptableItem item, int stage, bool ready, bool growing)
    {
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
            growthCoroutine = null;
        }

        if (item == null)
        {
            ClearPlantState();
            return;
        }

        // Ustawia plantedItem oraz uzupełnia growthStages z ScriptableItem
        SetPlantedItem(item);

        if (plantedItem == null)
            return;

        if (growthStages == null || growthStages.Count == 0)
            return;

        currentStage = Mathf.Clamp(stage, 0, growthStages.Count);
        isReadyToHarvest = ready;

        // W Twoim GrowthCycle currentStage zwiększa się PO ustawieniu sprite'a.
        // Czyli jeśli zapisano currentStage=1, to widoczny powinien być sprite index 0.
        int visibleStageIndex = Mathf.Clamp(currentStage - 1, 0, growthStages.Count - 1);

        SetSprite(growthStages[visibleStageIndex]);

        if (plantVisualsImage != null)
        {
            plantVisualsImage.gameObject.SetActive(true);
            plantVisualsImage.enabled = true;
        }
        else if (plantVisualsRenderer != null)
        {
            plantVisualsRenderer.gameObject.SetActive(true);
            plantVisualsRenderer.enabled = true;
        }
        else if (plantImage != null)
        {
            plantImage.enabled = true;
        }
        else if (plantRenderer != null)
        {
            plantRenderer.enabled = true;
        }

        if (growing && !isReadyToHarvest && currentStage < growthStages.Count)
        {
            growthCoroutine = StartCoroutine(GrowthCycle());
        }
    }

    public void ClearPlantState()
    {
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
            growthCoroutine = null;
        }

        plantedItem = null;
        currentStage = 0;
        isReadyToHarvest = false;

        if (growthStages != null)
            growthStages.Clear();

        if (plantVisualsImage != null)
        {
            plantVisualsImage.sprite = null;
            plantVisualsImage.gameObject.SetActive(false);
        }
        else if (plantVisualsRenderer != null)
        {
            plantVisualsRenderer.sprite = null;
            plantVisualsRenderer.gameObject.SetActive(false);
        }
        else if (plantImage != null)
        {
            plantImage.sprite = null;
            plantImage.enabled = false;
        }
        else if (plantRenderer != null)
        {
            plantRenderer.sprite = null;
            plantRenderer.enabled = false;
        }
    }
}