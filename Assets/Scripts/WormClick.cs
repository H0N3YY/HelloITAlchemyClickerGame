using UnityEngine;
using UnityEngine.EventSystems;

public class WormClick : MonoBehaviour, IPointerClickHandler
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindFirstObjectByType<SoundManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("WORM CLICKED: " + gameObject.name);

        if (soundManager != null)
        {
            soundManager.WormSquishSound();
        }

        gameObject.SetActive(false);
    }
    public void SquashWorm()
    {
        Debug.Log("WORM CLICKED: " + gameObject.name);

        if (soundManager != null)
        {
            soundManager.WormSquishSound();
        }

        gameObject.SetActive(false);
    }
}