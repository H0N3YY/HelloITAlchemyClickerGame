using System.Collections;
using UnityEngine;

public class WormSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] worms;
    [SerializeField] private float spawnInterval = 120f;

    private GameObject currentWorm;

    private void Start()
    {
        HideAllWorms();
        StartCoroutine(SpawnWormLoop());
    }

    private IEnumerator SpawnWormLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            ShowRandomWorm();
        }
    }

    private void HideAllWorms()
    {
        foreach (GameObject worm in worms)
        {
            if (worm != null)
            {
                worm.SetActive(false);
            }
        }
    }

    private void ShowRandomWorm()
    {
        if (currentWorm != null && currentWorm.activeSelf)
        {
            return;
        }

        if (worms == null || worms.Length == 0)
        {
            Debug.LogWarning("WormSpawner: Brakuje robaków w tablicy.");
            return;
        }

        int index = Random.Range(0, worms.Length);
        currentWorm = worms[index];

        if (currentWorm != null)
        {
            currentWorm.SetActive(true);
            currentWorm.transform.SetAsLastSibling();
        }
    }
}