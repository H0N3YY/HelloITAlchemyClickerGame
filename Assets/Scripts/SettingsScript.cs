using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations;
using  TMPro;

public class SettingsScript : MonoBehaviour
{
    public Slider masterVol, musicVol;
    public AudioMixer mainAudioMixer;
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", musicVol.value);
    }
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}
