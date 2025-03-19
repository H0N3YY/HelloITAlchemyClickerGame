using UnityEngine;

public class PlaySoundScript : MonoBehaviour
{
    public AudioSource workshopMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void toWorkshop()
    {
        workshopMusic.Play();
    }
    public void toBallroom()
    {
        workshopMusic.Stop();
    }
}
