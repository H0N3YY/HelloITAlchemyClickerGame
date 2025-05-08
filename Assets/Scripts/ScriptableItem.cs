using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    [Header("Podstawowe dane")]
    public string itemName;
    [TextArea(2, 5)] public string description;

    [Header("Wizualizacja")]
    public Sprite image;

    [Header("Właściwości logiki")]
    public ItemType type;
    public ActionType actionType;
    public bool stackable = true;
    public int manaCost;
    public int sellValue;
}

public enum ItemType
{
    Ingridient,
    Potion
}
public enum ActionType
{
    Drink,
    Craft

}