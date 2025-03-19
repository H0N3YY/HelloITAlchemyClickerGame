
using UnityEngine;
using UnityEngine.UI;

public class ButtonVisibilityScript : MonoBehaviour
{
    public Image imageToToggle;

    public void ToggleImageVisibility()
    {
        if (imageToToggle != null)
        {
            imageToToggle.gameObject.SetActive(!imageToToggle.gameObject.activeSelf);
        }
    }
}