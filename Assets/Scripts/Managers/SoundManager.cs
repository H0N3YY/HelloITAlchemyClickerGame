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
    public AudioSource scroll;
    public AudioSource lightFire;
    public AudioSource selling;
    public AudioSource unlockSound;
    public AudioSource petAnimal;
    public AudioSource boiling;
    public AudioClip wormSquish;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
            PlayerPrefs.SetFloat("MasterVolume", 1f);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 1f);

        PlayerPrefs.Save();
        UpdateVolumes();
    }

    private float ConvertSliderVolume(float value)
    {
        value = Mathf.Clamp01(value);
        return Mathf.Sqrt(value);
    }

    public void UpdateVolumes()
    {
        float masterSlider = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicSlider = PlayerPrefs.GetFloat("MusicVolume", 1f);

        float masterVolume = ConvertSliderVolume(masterSlider);
        float musicVolume = ConvertSliderVolume(musicSlider);

        if (musicSource != null)
        {
            musicSource.volume = masterVolume * musicVolume;
        }

        foreach (AudioSource source in GetComponentsInChildren<AudioSource>(true))
        {
            if (source != null && source != musicSource)
            {
                source.volume = masterVolume;
            }
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
        source.volume = ConvertSliderVolume(
    PlayerPrefs.GetFloat("MasterVolume", 1f)
);
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

    public void ScrollSound()
    {
        scroll.Play();
    }

    public void LightFireSound()
    {
        lightFire.Play();
    }

    public void SellingSound()
    {
        selling.Play();
    }

    public void UnlockSound()
    {
        unlockSound.Play();
    }

    public void PetAnimalSound()
    {
        petAnimal.Play();
    }

    public void BoilingSound()
    {
        boiling.Play();
    }

    public void WormSquishSound()
    {
        PlaySFX(wormSquish);
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
