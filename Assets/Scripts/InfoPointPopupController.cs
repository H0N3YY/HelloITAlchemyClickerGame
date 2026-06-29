using UnityEngine;

public class InfoPointPopupController : MonoBehaviour
{
    [SerializeField] private GameObject infoPointPopup;

    public void OpenPopup()
    {
        infoPointPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        infoPointPopup.SetActive(false);
    }
}