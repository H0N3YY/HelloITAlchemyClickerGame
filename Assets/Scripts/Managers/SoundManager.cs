using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public AudioSource button;
    public AudioSource gardenBackground;
    public AudioSource workshopMusic;
    public AudioSource mainRoomBackground;
    public AudioSource sphere;
    public void ButtonClick()
    {
        button.Play();
    }
    public void GardenPlay()
    {
        gardenBackground.Play();
    }
    public void GardenStop()
    {
        gardenBackground.Stop();
    }
    public void WorkshopPlay()
    {
        workshopMusic.Play();
    }
    public void WorkshopStop()
    {
        workshopMusic.Stop();
    }
    public void MainRoomPlay()
    {
        mainRoomBackground.Play();
    }
    public void MainRoomStop()
    {
        mainRoomBackground.Stop();
    }
    public void sphereClick()
    {
        sphere.Play();
    }

}
