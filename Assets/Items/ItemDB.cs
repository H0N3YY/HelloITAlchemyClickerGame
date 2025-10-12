using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ScriptableItem> allItems;

    public ScriptableItem GetItem(string id)
    {
        return allItems.Find(item => item.id == id);
    }
}
