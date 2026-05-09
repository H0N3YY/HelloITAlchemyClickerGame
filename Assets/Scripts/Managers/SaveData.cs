using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // --- CLICKER / PKT ---
    public double manaPoints;
    public double clickValue;
    public double idleClicks;

  

    // --- UPGRADY ---
    public List<UpgradeSave> upgrades = new List<UpgradeSave>();

    // --- INVENTORY ---
    public List<ItemSave> inventory = new List<ItemSave>();

    // --- ROŚLINY ---
    public List<PlantSave> plants = new List<PlantSave>();

    // --- KARTY ---
    public List<CardSave> cards = new List<CardSave>();

    // --- FIRE (PotionBrewer) ---
    public FireSave fireState = new FireSave();
}

[Serializable]
public class UpgradeSave
{
    public int index;
    public double cost;
    public bool isPurchased;
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
    public int plantIndex;
    public bool hasPlant;

    public string itemId;
    public int stage;
    public bool ready;
    public bool isGrowing;
}

[Serializable]
public class CardSave
{
    public string cardId;
    public bool isUnlocked;
    public bool firstTimeObtained;
}

[Serializable]
public class FireSave
{
    public bool fireActive;
    public float fireTimer;
}