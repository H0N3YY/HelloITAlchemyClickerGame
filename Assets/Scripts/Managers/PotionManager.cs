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
    private Coroutine sceneCoroutine;

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
        if (sceneCoroutine != null) StopCoroutine(sceneCoroutine);
        sceneCoroutine = StartCoroutine(ShowSceneTemporarily(sceneIndex: 2, duration: 15f, returnToIndex: 1));
    }

    private IEnumerator ShowSceneTemporarily(int sceneIndex, float duration, int returnToIndex)
    {
        // zapamiętaj aktywną scenę i jej rooty, żeby je schować
        Scene prevScene = SceneManager.GetActiveScene();
        var prevRoots = prevScene.GetRootGameObjects();
        foreach (var go in prevRoots) go.SetActive(false);

        // 1) Załaduj scenę docelową addytywnie
        var loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOp.isDone);

        Scene tempScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.SetActiveScene(tempScene);

        // 2) Odczekaj wymagany czas
        yield return new WaitForSeconds(duration);

        // 3) Wróć do sceny docelowej (np. 1)
        //    Jeśli nie jest załadowana (np. uruchomiłeś grę od innej sceny), doładuj ją.
        Scene target = SceneManager.GetSceneByBuildIndex(returnToIndex);
        if (!target.isLoaded)
        {
            var backLoad = SceneManager.LoadSceneAsync(returnToIndex, LoadSceneMode.Additive);
            yield return new WaitUntil(() => backLoad.isDone);
            target = SceneManager.GetSceneByBuildIndex(returnToIndex);
        }

        SceneManager.SetActiveScene(target);

        // Włącz ponownie obiekty poprzedniej sceny (jeśli wracasz do niej)
        if (target == prevScene)
        {
            foreach (var go in prevRoots) go.SetActive(true);
        }

        // 4) Wyładuj tymczasową scenę
        yield return SceneManager.UnloadSceneAsync(tempScene);

        sceneCoroutine = null;
    }        private IEnumerator Revert(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (volume != null && defaultProfile != null)
        {
            volume.profile = defaultProfile;
        }
        revertCoroutine = null;
    }
    }

