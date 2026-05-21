using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public AudioSource button;
    public AudioSource gardenBackground;
    public AudioSource workshopMusic;
    public AudioSource mainRoomBackground;
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
    public void ButtonClick()
    {
        button.Play();
    }
    public void SwitchRoomSound()
    {
        switchRoom.Play();
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

    public void deathMetalPotionMusic()
    {
        deathMetal.Play();
    }
    public void reggePotionMusic()
    {
       regge.Play();
    }



}
