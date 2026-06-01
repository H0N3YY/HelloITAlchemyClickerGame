using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField] 
    private GameObject sfxSourceGameObject;

    [Header("Music")]
    public AudioClip gardenBackground;
    public AudioClip workshopMusic;
    public AudioClip mainRoomBackground;

    [Header("SFX")]
    public AudioClip button;
    public AudioSource sphere;
    public AudioSource buying;
    public AudioSource takeItem;
    public AudioSource drinking;
    public AudioSource takePotion;
    public AudioSource feedPlant;
    public AudioSource pickupPlant;
    public AudioSource plantingPlant;
    public AudioSource finalCard;
    public AudioSource deathMetal;
    public AudioSource regge;
    public AudioSource switchRoom;
    public AudioSource putbackPotion;

    private void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", .5f);
    }

    public void UpdateVolumes()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", .5f);
        foreach (AudioSource source in sfxSourceGameObject.GetComponents<AudioSource>())
        {
            source.volume = PlayerPrefs.GetFloat("MasterVolume", .5f);
        }
    }

    #region Music functions
    public void GardenPlay()
    {
        PlayMusic(gardenBackground);
    }

    public void WorkshopPlay()
    {
        PlayMusic(workshopMusic);
    }
    public void MainRoomPlay()
    {
        PlayMusic(mainRoomBackground);
    }

    private void PlayMusic(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
    #endregion

    #region SFX functions
    private IEnumerator SFXCoroutine(AudioSource source)
    {
        source.volume = PlayerPrefs.GetFloat("MasterVolume", .5f);
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        source.Stop();
        Destroy(source);
    }

    private void PlaySFX(AudioClip clip)
    {
        AudioSource source = sfxSourceGameObject.AddComponent<AudioSource>();
        source.clip = clip;

        StartCoroutine(SFXCoroutine(source));
    }

    public void ButtonClick()
    {
        PlaySFX(button);
    }
    public void SwitchRoomSound()
    {
        switchRoom.Play();
    }
    public void sphereClick()
    {
        sphere.Play();
    }
    public void buyingItem()
    {
       buying.Play();
    }
    public void TakeItemSound()
    {
        takeItem.Play();
    }
    public void TakePotionSound()
    {
        takePotion.Play();
    }
    public void PutBackPotionSound()
    {
        putbackPotion.Play();
    }
    public void drinkPotion()
    {
        drinking.Play();
    }
    public void feedPlantSound()
    {
        feedPlant.Play();
    }
    public void pickUpPlantSound()
    {
        pickupPlant.Play();
    }
    public void plantingPlantSound()
    {
        plantingPlant.Play();
    }

    public void finalCardSound()
    {
        finalCard.Play();
    }

    /*
     * Generalnie to w PotionManager te 2 maj¹ swoje wywo³anie, a mi (Anirze) nie chce sie jebac ze znacznymi zmianami w kodzie XDDDD
     * Niech tak pozostanie
     * P.S. Jebac majonez Kielecki
     * 
    public void deathMetalPotionMusic()
    {
        deathMetal.Play();
    }
    public void reggePotionMusic()
    {
       regge.Play();
    }*/
    #endregion


}
