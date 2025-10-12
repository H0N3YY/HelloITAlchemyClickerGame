using UnityEngine;

public class CursorManager : MonoBehaviour
{   
    [Header("Default Cursor")]
    public Texture2D defaultCursor;
    public Vector2 defaultHotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

     [Header("Furry Cursor")]
    public Texture2D furryCursor;
    public Vector2 furryHotspot = new Vector2(0, 0);


     public static CursorManager Instance;
   void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetDefaultCursor();
    }

    public void SetFurryCursor()
    {
        Cursor.SetCursor(furryCursor, furryHotspot, cursorMode);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, cursorMode);
    }
}
