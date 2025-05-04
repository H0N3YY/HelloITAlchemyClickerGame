using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrowth : MonoBehaviour
{
    [Header("Gwowth Stages Sprites")]
    public List<Sprite> growthStages;
    [Header("Plant Visuals (SpriteRenderer)")]
    public SpriteRenderer plantVisualsRenderer;

    public GameObject draggableItemPrefab;

    [Header("Item Spawn Point")]
    public Transform spawnPoint;

    [Header("Growth Interval")]
    public float growthInterval = 20f;

    private Image plantImage;
    private SpriteRenderer plantRenderer;

    private int currentStage = 0;
    private bool isReadyToHarvest = false;

    private void Awake()
    {

        plantImage = GetComponent<Image>();
        plantRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartGrowth()
    {
        StartCoroutine(GrowthCycle());
    }

    private IEnumerator GrowthCycle()
    {
        while (currentStage < growthStages.Count)
        {
            SetSprite(growthStages[currentStage]);
            currentStage++;

            if (currentStage >= growthStages.Count)
            {
                break;
            }

            yield return new WaitForSeconds(growthInterval);
        }


        isReadyToHarvest = true;
        Debug.Log("Roślinka jest gotowa do zebrania!");
    }

    // private void SetSprite(Sprite newSprite)
    // {
    //     if (plantImage != null)
    //     {
    //         plantImage.sprite = newSprite;
    //     }
    //     else if (plantRenderer != null)
    //     {
    //         plantRenderer.sprite = newSprite;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Nie znaleziono Image ani SpriteRenderer!");
    //     }
    // }

    private void SetSprite(Sprite newSprite)
    {

        if (plantVisualsRenderer != null)
        {

            Color stageColor = Color.white;

            if (currentStage == 0)
                stageColor = Color.green;
            else if (currentStage == 1)
                stageColor = Color.yellow;
            else if (currentStage == 2)
                stageColor = Color.red;


            plantVisualsRenderer.color = stageColor;
            Debug.Log($"Zmiana koloru na {stageColor} dla etapu {currentStage + 1}");
        }
        else if (plantImage != null)
        {
            plantImage.sprite = newSprite;
        }
        else if (plantRenderer != null)
        {
            plantRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono komponentu");
        }
    }


    public void Harvest()
    {
        if (!isReadyToHarvest)
        {
            Debug.Log("Jeszcze nie można zebrać roślinki");
            return;
        }

        Debug.Log("Zbieram roślinkę!");


        gameObject.SetActive(false);


        if (draggableItemPrefab != null && spawnPoint != null)
        {
            Instantiate(draggableItemPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Nie przypięto prefab'u lub punktu spawnu");
        }
    }
}
