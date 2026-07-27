using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private Slider masterVol;
    [SerializeField] private Slider musicVol;
    [SerializeField] private SoundManager soundManager;

    public void ChangeMasterVolume(float value)
    {
        value = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        soundManager?.UpdateVolumes();
    }

    public void ChangeMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        soundManager?.UpdateVolumes();
    }

    private void Start()
    {
        float master = Mathf.Clamp01(
            PlayerPrefs.GetFloat("MasterVolume", 1f)
        );

        float music = Mathf.Clamp01(
            PlayerPrefs.GetFloat("MusicVolume", 1f)
        );

        masterVol.SetValueWithoutNotify(master);
        musicVol.SetValueWithoutNotify(music);

        soundManager?.UpdateVolumes();
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}