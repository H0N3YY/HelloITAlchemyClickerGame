using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableCard", menuName = "Scriptable Objects/ScriptableCard")]
public class ScriptableCard : ScriptableObject
{
    [Header("Basic data")]
    public string id;
    public string itemName;

    [Header("Card visuals")]
    public Sprite artwork;

    [Header("Logic properties")]
    public bool isUnlocked = false;
    public bool firstTimeObtained = true;
}
