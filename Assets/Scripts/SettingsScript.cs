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
        PlayerPrefs.SetFloat("MasterVol", masterVol.value);
        PlayerPrefs.Save();
        soundManager.UpdateVolumes();
    }
    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVol", masterVol.value);
        PlayerPrefs.Save();
        soundManager.UpdateVolumes();
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private void Start()
    {
        masterVol.value = PlayerPrefs.GetFloat("MasterVol", .5f);
        musicVol.value = PlayerPrefs.GetFloat("MusicVol", .5f);
    }
}
