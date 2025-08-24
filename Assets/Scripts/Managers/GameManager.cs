using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


            ApplySaveData(SaveSystem.LoadGame());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
         //uruchomi AutoSave co 30 sekund
       InvokeRepeating(nameof(AutoSave), 30f, 30f);
    }

    private void AutoSave()
   {
        SaveSystem.SaveGame(CollectSaveData());
       Debug.Log("AutoSave wykonany!");
    }
    private void Save()
    {
        SaveSystem.SaveGame(CollectSaveData());
        Debug.Log("Save wykonany!");
    }
     public void SaveAndExit()
    {
        if (GameManager.Instance != null)
        {
            SaveSystem.SaveGame(GameManager.Instance.CollectSaveData());
            Debug.Log("Gra zapisana, wychodzę...");
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

        // --- CLICKER ---
        data.manaPoints = manaPoints;
        data.clickValue = clickValue;
        data.idleClicks = idleClicks;

        // --- UPGRADY ---
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

        return data;
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
}


    public double GetEffectiveIdle() => idleClicks * idleMultiplier;
    public double GetEffectiveClick() => clickValue * clickMultiplier;
}
