using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Podstawowe dane clickera")]
    public double manaPoints = 0;
    public double idleClicks = 0;
    public double clickValue = 1;

    public double idleMultiplier = 1.0;
    public double clickMultiplier = 1.0;
    [Header("Baza itemów")]
    public ItemDatabase itemDatabase;

    [Header("Referencje")]
    public GameObject DragableItemPrefab;
    public magicBallScript magicBall;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<PlantGrowth> plants = new List<PlantGrowth>();
    [Header("Karty")]
    public List<ScriptableCard> cards = new List<ScriptableCard>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


            ApplySaveData(SaveSystem.LoadGame()); // loading save
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(AutoSave), 30f, 30f);
    }

    private void AutoSave()
    {
        SaveSystem.SaveGame(CollectSaveData());
        Debug.Log("AutoSave wykonany");
    }
    private void Save()
    {
        SaveSystem.SaveGame(CollectSaveData());
        Debug.Log("Save wykonany");
    }
    public void SaveAndExit()
    {
        if (GameManager.Instance != null)
        {
            SaveSystem.SaveGame(GameManager.Instance.CollectSaveData());
            Debug.Log("Gra zapisana, exiting");
        }
        else
        {
            Debug.LogWarning("Brak GameManager.Instance nic nie zapisano");
        }

        Application.Quit();
    }


    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveSystem.SaveGame(CollectSaveData());
        }
    }


    // ========================
    // SAVE / LOAD
    // ========================
    public SaveData CollectSaveData()
    {
        SaveData data = new SaveData();

        // --- PKT ---
        data.manaPoints = manaPoints;
        data.clickValue = clickValue;
        data.idleClicks = idleClicks;

        // --- UPGRADES ---
        if (magicBall != null && magicBall.upgrades != null)
        {
            for (int i = 0; i < magicBall.upgrades.Count; i++)
            {
                var u = magicBall.upgrades[i];
                data.upgrades.Add(new UpgradeSave
                {
                    index = i,
                    cost = u.cost
                });
            }
        }

        // --- INVENTORY ---
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slot = inventorySlots[i];
            if (slot == null)
            {
                Debug.LogWarning($"[SAVE] Slot {i} w GameManager nie jest przypisany");
                continue;
            }

            var di = slot.GetComponentInChildren<DragableItem>(true);
            if (di != null && di.item != null)
            {
                data.inventory.Add(new ItemSave
                {
                    itemId = di.item.id,
                    count = di.count,
                    slotIndex = i
                });
                Debug.Log($"[SAVE] Slot {i}: zapisano {di.item.itemName} x{di.count}");
            }
            else
            {
                Debug.Log($"[SAVE] Slot {i} pusty");
            }
        }

        Debug.Log($"[SAVE] Łącznie zapisano {data.inventory.Count} itemów z {inventorySlots.Count} slotów.");
        // --- ROŚLINY ---

        //Debug.Log($"[SAVE][PLANTS] Sekcja roślin uruchomiona. plants == null: {plants == null}");

        //if (plants == null)
        //{
        //Debug.LogError("[SAVE][PLANTS] Lista plants jest NULL!");
        //}
        //else
        //{
        //    Debug.Log($"[SAVE][PLANTS] plants.Count = {plants.Count}");
        //}

        for (int i = 0; i < plants.Count; i++)
        {
            PlantGrowth plant = plants[i];

            if (plant == null)
            {
                Debug.LogWarning($"[SAVE][PLANTS] Plant index {i} jest NULL w liście GameManager.plants");
                continue;
            }

            Debug.Log($"[SAVE][PLANTS] Sprawdzam plant index {i}. " +
                      $"GameObject={plant.gameObject.name}, " +
                      $"PlantedItem={(plant.PlantedItem != null ? plant.PlantedItem.itemName : "NULL")}, " +
                      $"ItemID={(plant.PlantedItem != null ? plant.PlantedItem.id : "NULL")}, " +
                      $"CurrentStage={plant.CurrentStage}, " +
                      $"Ready={plant.IsReadyToHarvest}, " +
                      $"Growing={plant.IsGrowing}");

            PlantSave plantSave = new PlantSave
            {
                plantIndex = i,
                hasPlant = plant.PlantedItem != null,
                itemId = plant.PlantedItem != null ? plant.PlantedItem.id : "",
                stage = plant.CurrentStage,
                ready = plant.IsReadyToHarvest,
                isGrowing = plant.IsGrowing
            };

            data.plants.Add(plantSave);

            Debug.Log($"[SAVE][PLANTS] Dodano do SaveData: " +
                      $"plantIndex={plantSave.plantIndex}, " +
                      $"hasPlant={plantSave.hasPlant}, " +
                      $"itemId='{plantSave.itemId}', " +
                      $"stage={plantSave.stage}, " +
                      $"ready={plantSave.ready}, " +
                      $"isGrowing={plantSave.isGrowing}");
        }

        Debug.Log($"[SAVE][PLANTS] Koniec zapisu roślin. Zapisano wpisów plants: {data.plants.Count}");
        // --- KARTY ---
        for (int i = 0; i < cards.Count; i++)
        {
            ScriptableCard card = cards[i];

            if (card == null)
            {
                Debug.LogWarning($"[SAVE] Card {i} nie jest przypisana");
                continue;
            }

            data.cards.Add(new CardSave
            {
                cardId = card.id,
                isUnlocked = card.isUnlocked,
                firstTimeObtained = card.firstTimeObtained
            });

            Debug.Log($"[SAVE] Card {card.id}: unlocked={card.isUnlocked}, firstTime={card.firstTimeObtained}");
        }
        return data;
    }
    private ScriptableCard GetCardById(string id)
    {
        foreach (ScriptableCard card in cards)
        {
            if (card != null && card.id == id)
                return card;
        }

        return null;
    }




    public void ApplySaveData(SaveData data)
    {
        if (data == null) return;

        Debug.Log($"[LOAD] Wczytywanie save, inventory ma {data.inventory.Count} itemów");


        // Clicker
        manaPoints = data.manaPoints;
        clickValue = (data.clickValue <= 0) ? 1 : data.clickValue;
        idleClicks = data.idleClicks;

        // Upgrady
        if (magicBall != null && data.upgrades != null)
        {
            for (int i = 0; i < data.upgrades.Count; i++)
            {
                if (i < magicBall.upgrades.Count)
                {
                    magicBall.upgrades[i].cost = data.upgrades[i].cost;
                    if (magicBall.upgrades[i].priceText != null)
                        magicBall.upgrades[i].priceText.text = ((int)data.upgrades[i].cost).ToString();
                }
            }
        }

        // Inventory
        foreach (var slot in inventorySlots)
        {
            foreach (Transform child in slot.transform)
                Destroy(child.gameObject);
        }

        foreach (var itemSave in data.inventory)
        {
            if (itemSave.slotIndex < inventorySlots.Count)
            {
                var slot = inventorySlots[itemSave.slotIndex];
                GameObject newItem = Instantiate(DragableItemPrefab, slot.transform);

                DragableItem di = newItem.GetComponent<DragableItem>();
                if (di != null)
                {
                    ScriptableItem itemAsset = itemDatabase.GetItem(itemSave.itemId);
                    if (itemAsset == null)
                    {
                        Debug.LogError($"[LOAD] Brak itemu o ID: {itemSave.itemId} w ItemDatabase!");
                        continue;
                    }

                    di.item = itemAsset;
                    di.count = itemSave.count;
                    di.InitialiseItem(itemAsset);

                    Debug.Log($"[LOAD] Slot {itemSave.slotIndex}: {itemAsset.id} x{di.count}");
                }
            }
        }
        // --- ROŚLINY ---
        if (data.plants != null)
        {
            foreach (PlantSave plantSave in data.plants)
            {
                if (plantSave.plantIndex < 0 || plantSave.plantIndex >= plants.Count)
                {
                    Debug.LogWarning($"[LOAD] Plant index poza zakresem: {plantSave.plantIndex}");
                    continue;
                }

                PlantGrowth plant = plants[plantSave.plantIndex];

                if (plant == null)
                {
                    Debug.LogWarning($"[LOAD] Plant {plantSave.plantIndex} nie jest przypisany");
                    continue;
                }

                if (!plantSave.hasPlant)
                {
                    plant.ClearPlantState();
                    continue;
                }

                Debug.Log($"[LOAD][PLANTS] Szukam itemu rośliny po ID: '{plantSave.itemId}'");

                ScriptableItem itemAsset = itemDatabase.GetItem(plantSave.itemId);

                if (itemAsset == null)
                {
                    Debug.LogError($"[LOAD][PLANTS] Brak itemu rośliny o ID: '{plantSave.itemId}' w ItemDatabase!");
                    continue;
                }

                Debug.Log($"[LOAD][PLANTS] Znaleziono item rośliny: {itemAsset.itemName}, ID={itemAsset.id}");

                plant.LoadPlantState(
                    itemAsset,
                    plantSave.stage,
                    plantSave.ready,
                    plantSave.isGrowing
                );

                Debug.Log($"[LOAD] Plant {plantSave.plantIndex}: {itemAsset.id}, stage={plantSave.stage}, ready={plantSave.ready}");
            }
        }
        // --- KARTY ---
        if (data.cards != null)
        {
            foreach (CardSave cardSave in data.cards)
            {
                ScriptableCard card = GetCardById(cardSave.cardId);

                if (card == null)
                {
                    Debug.LogWarning($"[LOAD] Brak karty o ID: {cardSave.cardId}");
                    continue;
                }

                card.isUnlocked = cardSave.isUnlocked;
                card.firstTimeObtained = cardSave.firstTimeObtained;

                Debug.Log($"[LOAD] Card {card.id}: unlocked={card.isUnlocked}, firstTime={card.firstTimeObtained}");
            }
        }
    }


    public double GetEffectiveIdle() => idleClicks * idleMultiplier;
    public double GetEffectiveClick() => clickValue * clickMultiplier;
}
