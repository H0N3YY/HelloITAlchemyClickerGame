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
        StartCoroutine(HomlessRoutine());
    }

    private IEnumerator HomlessRoutine()
    {
        // Załaduj scenę 2 addytywnie
        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        // Przełącz aktywną scenę na 2
        Scene tempScene = SceneManager.GetSceneByBuildIndex(2);
        SceneManager.SetActiveScene(tempScene);

        // Odczekaj 15 sekund
        yield return new WaitForSeconds(15f);

        // Załaduj scenę 1 (jeśli jeszcze jej nie ma)
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
            yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // Ustaw scenę 1 jako aktywną
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));

        // Wyładuj scenę 2
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

