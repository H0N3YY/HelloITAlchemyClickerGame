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

    public void ChangeMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        if (soundManager != null)
            soundManager.UpdateVolumes();
    }

    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        if (soundManager != null)
            soundManager.UpdateVolumes();
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    private void Start()
    {
        masterVol.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", .5f));
        musicVol.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", .5f));

        if (soundManager != null)
        {
            soundManager.UpdateVolumes();
        }
    }
}
