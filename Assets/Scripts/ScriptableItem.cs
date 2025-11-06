using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    [Header("Basic data")]
    public string id;
    public string itemName;
    [TextArea(2, 5)] public string description;

    [Header("Visuals")]
    public Sprite image;

    public Sprite plantStage1;

    public Sprite PlantStage2;

    public Sprite PlantStage3;
    [Header("Logic properties")]
    public ItemType type;
    public ActionType actionType;
    public bool stackable = true;
    public int manaCost;
    public int sellValue;


    [Header("Effect to trigger on use")]
    public PotionEffect effectToTrigger = PotionEffect.None;
}

public enum ItemType
{
    Ingridient,
    Potion,
    Seed,
}
public enum ActionType
{
    Drink,
    Craft,
    Plant

}

public enum PotionEffect
{
    None = 0,
    Blind,
    Reggae,
    Metal,
    Photophobia,
    Unstable,
    Homeless,
    Antidote,
    Weaken1,
    Weaken2,
    Weaken3,
    BoostIdle1,
    BoostIdle2,
    BoostIdle3,
    ClickBoost1,
    ClickBoost2,
    ClickBoost3,
    Chaos,
    Furry
}