using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
public class PotionManager : MonoBehaviour
{

    public Volume volume;                   
    public VolumeProfile defaultProfile; 
    public VolumeProfile metalProfile;    
    public VolumeProfile photofobiaProfile; 

    private Coroutine revertCoroutine;
   

    public void metalPotion()
    {
        if (volume != null && metalProfile != null)
        {
            volume.profile = metalProfile;
            if (revertCoroutine != null)
                StopCoroutine(revertCoroutine);

            revertCoroutine = StartCoroutine(Revert(30f));
        }
    }

    public void photofobiaPotion()
    {
        if (volume != null && photofobiaProfile != null)
        {
            volume.profile = photofobiaProfile;
            if (revertCoroutine != null)
                StopCoroutine(revertCoroutine);

            revertCoroutine = StartCoroutine(Revert(15f));
        }
    }
    public void HomlessPotion()
    {
        StartCoroutine(HomlessRoutine());
    }

    private IEnumerator HomlessRoutine()
    {
        Scene originalScene = SceneManager.GetActiveScene();

        var rootObjects = originalScene.GetRootGameObjects();
        foreach (var go in rootObjects)
            if (!go.name.Contains("Managers"))
                go.SetActive(false);

        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        Scene tempScene = SceneManager.GetSceneByBuildIndex(2);
        SceneManager.SetActiveScene(tempScene);

        yield return new WaitForSeconds(15f);
        Debug.Log("Mineło 15 sec");

        SceneManager.SetActiveScene(originalScene);
        
        foreach (var go in rootObjects)
            go.SetActive(true);

        yield return SceneManager.UnloadSceneAsync(tempScene);
    }
            private IEnumerator Revert(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (volume != null && defaultProfile != null)
        {
            volume.profile = defaultProfile;
        }
        revertCoroutine = null;
    }
    }

