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
     public void StartSetup()
    {
        SceneManager.LoadSceneAsync(2);
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

