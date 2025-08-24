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
    private Coroutine manaWeakenCoroutine;
    private Coroutine boostCoroutine;


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
    public void WeakenPotionTier1()
    {
        WeakenPotion(1);
    }

    public void WeakenPotionTier2()
    {
        WeakenPotion(2);
    }

    public void WeakenPotionTier3()
    {
        WeakenPotion(3);
    }
    void WeakenPotion(int tier)
    {
        if (manaWeakenCoroutine != null)
            StopCoroutine(manaWeakenCoroutine);

        manaWeakenCoroutine = StartCoroutine(ManaWeakenRoutine(tier));
    }
    public void BoostPotionTier1()
    {
        BoostPotion(1);
    }
    public void BoostPotionTier2()
    {
        BoostPotion(2);
    }
    public void BoostPotionTier3()
    {
        BoostPotion(3);
    }
    void BoostPotion(int tier)
    {
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(BoostRoutine(tier));
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
    private IEnumerator ManaWeakenRoutine(int tier)
    {
        float duration = 60f;
        double weakenMultiplier = 1.0;

        switch (tier)
        {
            case 1: weakenMultiplier = 0.75; break; // 75% mocy
            case 2: weakenMultiplier = 0.5; break;  // 50% mocy
            case 3: weakenMultiplier = 0.25; break; // 25% mocy
            default: weakenMultiplier = 1.0; break;
        }

        // ustaw mnożniki
        GameManager.Instance.idleMultiplier *= weakenMultiplier;
        GameManager.Instance.clickMultiplier *= weakenMultiplier;

        yield return new WaitForSeconds(duration);

        // przywróć stan
        GameManager.Instance.idleMultiplier /= weakenMultiplier;
        GameManager.Instance.clickMultiplier /= weakenMultiplier;

        Debug.Log("Potka osłabienia skończyła się.");
        manaWeakenCoroutine = null;
    }
    private IEnumerator BoostRoutine(int tier)
    {
        double boostMultiplier = 1.0;
        float duration = 30f;


        switch (tier)
        {
            case 1: boostMultiplier = 1.5; break; // +50% CPS
            case 2: boostMultiplier = 2.0; break; // +100% CPS
            case 3: boostMultiplier = 3.0; break; // +200% CPS
            default: boostMultiplier = 1.0; break; // brak efektu
        }


        GameManager.Instance.idleMultiplier *= boostMultiplier;

        yield return new WaitForSeconds(duration);


        GameManager.Instance.idleMultiplier /= boostMultiplier;

        Debug.Log("Boost potka skończyła się.");
        boostCoroutine = null;
    }
}

