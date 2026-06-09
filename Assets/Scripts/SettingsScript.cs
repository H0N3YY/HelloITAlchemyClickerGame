using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations;
using  TMPro;

public class SettingsScript : MonoBehaviour
{
    public Slider masterVol, musicVol;

    [SerializeField]
    private SoundManager soundManager;

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVol.value);
        PlayerPrefs.Save();
        soundManager.UpdateVolumes();
    }
    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVol.value);
        PlayerPrefs.Save();
        soundManager.UpdateVolumes();
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private void Start()
    {
        masterVol.value = PlayerPrefs.GetFloat("MasterVolume", .5f);
        musicVol.value = PlayerPrefs.GetFloat("MusicVolume", .5f);

        if (soundManager != null)
        {
            soundManager.UpdateVolumes();
        }
    }
}
