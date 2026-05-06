using UnityEngine;
using System.IO;


public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Zapisano grę: " + path);
        Debug.Log($"[SAVE SYSTEM] Plants count: {(data.plants != null ? data.plants.Count : -1)}");
        Debug.Log($"[SAVE SYSTEM] JSON:\n{json}");
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("no save file -> new game");
            return new SaveData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}