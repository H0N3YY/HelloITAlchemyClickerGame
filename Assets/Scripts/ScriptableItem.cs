using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public ActionType actionType;
    public bool stackable = true;
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