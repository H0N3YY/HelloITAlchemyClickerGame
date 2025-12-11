using System;
using System.Collections.Generic;


[Serializable]
public class SaveData
{
    // --- CLICKER ---
    public double manaPoints;
    public double clickValue;
    public double idleClicks;

    // --- UPGRADY ---
    public List<UpgradeSave> upgrades = new List<UpgradeSave>();

    // --- INVENTORY ---
    public List<ItemSave> inventory = new List<ItemSave>();

    // --- ROŚLINY ---
    public List<PlantSave> plants = new List<PlantSave>();

    // --- FIRE (PotionBrewer) ---
    public FireSave fireState = new FireSave();
}

[Serializable]
public class UpgradeSave
{
    public int index;
    public double cost;
}

[Serializable]
public class ItemSave
{
    public string itemId;
    public int count;
    public int slotIndex;
}

[Serializable]
public class PlantSave
{
    public string itemId;
    public int stage;
    public bool ready;
}

[Serializable]
public class FireSave
{
    public bool fireActive;
    public float fireTimer;
}
