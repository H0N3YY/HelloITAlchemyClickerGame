using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class PotionManager : MonoBehaviour
{
    [SerializeField] private MemoryCardCounterScript memoryCardCounterScript;
    public Volume volume;
    public VolumeProfile defaultProfile;
    [Header("Post Processing Profiles")]
    public VolumeProfile reggaeProfile;
    public VolumeProfile metalProfile;
    public VolumeProfile blindProfile;
    public VolumeProfile photofobiaProfile;
    private Coroutine revertCoroutine;
    private Coroutine reggaeCoroutine;
    private Coroutine manaWeakenCoroutine;
    private Coroutine boostIdleCoroutine;
    private Coroutine clickBoostCoroutine;
    [Header("Reggae / Background swap")]
    public Image[] backgroundImages = new Image[3];
    public Sprite[] replacementSprites = new Sprite[3]; // Ensure this array has the same length as backgroundImages
    [Header("Card display")]
    [SerializeField] private Memory_Card_Popup memoryCardPopup;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip reggaeClip;
    public AudioClip metalMusicClip;
    [Range(0f, 1f)] public float targetMusicVolume = 0.8f;
    public float musicFadeTime = 0.35f;


    [Header("Unstable Potion")]
    public Transform cameraPivot;
    private Coroutine unstableCoroutine;

    [Header("Furry Potion")]
    public float furryDuration = 20f;
    private Coroutine furryCoroutine;

    public void UseItem(ScriptableItem item)
    {
        if (!item) return;

        // 1) Efekt potki
        ExecuteEffect(item.effectToTrigger);

        // 2) Obsługa karty powiązanej z tym itemem (pierwsze wypicie -> popup)
        var card = item.associatedCard;
        if (!card) return;

        Debug.Log($"[PotionManager] UseItem -> associated card = {card.itemName}", this);

        // jeśli karta jeszcze nie była odblokowana – odblokuj ją
        if (!card.isUnlocked)
        {
            card.isUnlocked = true;
            memoryCardCounterScript.RefreshCounter();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(card);
#endif
        }

        // popup tylko przy pierwszym wypiciu potki związanej z tą kartą
        if (card.firstTimeObtained)
        {
            Debug.Log($"[PotionManager] First time obtained card: {card.itemName}", this);

            if (memoryCardPopup != null)
            {
                Debug.Log("[PotionManager] Calling memoryCardPopup.ShowCard()", this);
                memoryCardPopup.ShowCard(card); // pokazuje kartę na 8 sekund
            }
            else
            {
                Debug.LogWarning("PotionManager: Memory_Card_Popup nie jest podpięty w inspectorze.", this);
            }

            card.firstTimeObtained = false;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(card);
#endif
        }
    }


    public void ChaosPotion()
    {
        System.Action[] possibleEffects = new System.Action[]
        {
        UnstablePotion,
        blindPotion,
        reggaePotion,
        metalPotion,
        photofobiaPotion,
        HomlessPotion,
        WeakenPotionTier1,
        WeakenPotionTier2,
        WeakenPotionTier3,
        BoostIdlePotionTier1,
        BoostIdlePotionTier2,
        BoostIdlePotionTier3,
        ClickBoostPotionTier1,
        ClickBoostPotionTier2,
        ClickBoostPotionTier3,
        FurryPotion
        };

        int index = Random.Range(0, possibleEffects.Length);
        Debug.Log($"Chaos Potion: #{index}: {possibleEffects[index].Method.Name}");
        possibleEffects[index]?.Invoke();
    }


    public void UnstablePotion()
    {
        if (unstableCoroutine != null)
            StopCoroutine(unstableCoroutine);

        unstableCoroutine = StartCoroutine(UnstableRoutine(20f));
    }

    public void FurryPotion()
    {
        if (furryCoroutine != null)
            StopCoroutine(furryCoroutine);

        furryCoroutine = StartCoroutine(FurryRoutine(20f));
    }




    public void blindPotion()
    {
        if (volume != null && blindProfile != null)
        {
            volume.profile = blindProfile;
            if (revertCoroutine != null)
                StopCoroutine(revertCoroutine);

            revertCoroutine = StartCoroutine(Revert(15f));
        }
    }

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
    public void reggaePotion()
    {
        if (volume != null && reggaeProfile != null)
        {
            volume.profile = reggaeProfile;
            if (revertCoroutine != null)
                StopCoroutine(revertCoroutine);

            revertCoroutine = StartCoroutine(Revert(21f));
            if (reggaeCoroutine != null)
                StopCoroutine(reggaeCoroutine);

            reggaeCoroutine = StartCoroutine(Reggae(20f));
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

    public void AntidotePotion()
    {
        if (unstableCoroutine != null)
        {
            StopCoroutine(unstableCoroutine);
            unstableCoroutine = null;

            if (cameraPivot)
                cameraPivot.rotation = Quaternion.identity;

            Debug.Log("Antidotum: worked");
        }


        if (manaWeakenCoroutine != null)
        {
            StopCoroutine(manaWeakenCoroutine);
            manaWeakenCoroutine = null;


            GameManager.Instance.idleMultiplier = 1.0;
            GameManager.Instance.clickMultiplier = 1.0;

            Debug.Log("Antidotum: worked");
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
    public void BoostIdlePotionTier1()
    {
        BoostIdlePotion(1);
    }
    public void BoostIdlePotionTier2()
    {
        BoostIdlePotion(2);
    }
    public void BoostIdlePotionTier3()
    {
        BoostIdlePotion(3);
    }
    void BoostIdlePotion(int tier)
    {
        if (boostIdleCoroutine != null)
            StopCoroutine(boostIdleCoroutine);

        boostIdleCoroutine = StartCoroutine(BoostIdleRoutine(tier));
    }
    public void ClickBoostPotionTier1()
    {
        ClickBoostPotion(1);
    }

    public void ClickBoostPotionTier2()
    {
        ClickBoostPotion(2);
    }

    public void ClickBoostPotionTier3()
    {
        ClickBoostPotion(3);
    }

    private void ClickBoostPotion(int tier)
    {
        if (clickBoostCoroutine != null)
            StopCoroutine(clickBoostCoroutine);

        clickBoostCoroutine = StartCoroutine(ClickBoostRoutine(tier));
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
        Debug.Log("15 secoonds pased");

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
    private IEnumerator BoostIdleRoutine(int tier)
    {
        double boostIdleMultiplier = 1.0;
        float duration = 30f;


        switch (tier)
        {
            case 1: boostIdleMultiplier = 1.5; break; // +50% CPS
            case 2: boostIdleMultiplier = 2.0; break; // +100% CPS
            case 3: boostIdleMultiplier = 3.0; break; // +200% CPS
            default: boostIdleMultiplier = 1.0; break;
        }


        GameManager.Instance.idleMultiplier *= boostIdleMultiplier;

        yield return new WaitForSeconds(duration);


        GameManager.Instance.idleMultiplier /= boostIdleMultiplier;

        Debug.Log("Boost potka skończyła się.");
        boostIdleCoroutine = null;
    }
    private IEnumerator ClickBoostRoutine(int tier)
    {
        double boostMultiplier = 1.0;
        float duration = 30f;


        switch (tier)
        {
            case 1: boostMultiplier = 1.5; break; // +50% do klikania
            case 2: boostMultiplier = 2.0; break; // +100% do klikania
            case 3: boostMultiplier = 3.0; break; // +200% do klikania
            default: boostMultiplier = 1.0; break;
        }


        GameManager.Instance.clickMultiplier *= boostMultiplier;

        yield return new WaitForSeconds(duration);


        GameManager.Instance.clickMultiplier /= boostMultiplier;

        Debug.Log("Click Boost potka skończyła się.");
        clickBoostCoroutine = null;
    }
    private IEnumerator Reggae(float duration)
    {
        // --- Background Swap ---
        Sprite[] originals = new Sprite[backgroundImages.Length];
        for (int i = 0; i < backgroundImages.Length && i < replacementSprites.Length; i++)
        {
            if (!backgroundImages[i]) continue;
            originals[i] = backgroundImages[i].sprite;
            if (replacementSprites[i])
                backgroundImages[i].sprite = replacementSprites[i];
        }


        AudioClip prevClip = null;
        float prevVol = 1f;
        bool prevWasPlaying = false;

        if (musicSource)
        {
            prevClip = musicSource.clip;
            prevVol = musicSource.volume;
            prevWasPlaying = musicSource.isPlaying;

            // fade out 
            if (prevWasPlaying && prevClip != reggaeClip)
                yield return StartCoroutine(FadeAudio(musicSource, 0f, musicFadeTime));


            musicSource.clip = reggaeClip;
            musicSource.loop = true;
            musicSource.Play();
            yield return StartCoroutine(FadeAudio(musicSource, targetMusicVolume, musicFadeTime));
        }

        yield return new WaitForSeconds(duration);


        if (musicSource)
        {
            yield return StartCoroutine(FadeAudio(musicSource, 0f, musicFadeTime));
            musicSource.Stop();
            musicSource.clip = prevClip;
            musicSource.volume = prevVol;
            if (prevClip && prevWasPlaying) musicSource.Play();
        }

        // --- Revert Backgrounds ---
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            if (backgroundImages[i])
                backgroundImages[i].sprite = originals[i];
        }

        reggaeCoroutine = null;
    }
    private IEnumerator UnstableRoutine(float duration)
    {
        if (!cameraPivot)
        {
            Debug.LogWarning("Niestabilna: Brak przypisanego cameraPivot.");
            yield break;
        }

        const float rotateTime = 0.35f;
        Quaternion startRot = cameraPivot.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0f, 0f, 180f);


        float t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            cameraPivot.rotation = Quaternion.Slerp(startRot, targetRot, t / rotateTime);
            yield return null;
        }
        cameraPivot.rotation = targetRot;


        yield return new WaitForSeconds(duration);


        t = 0f;
        while (t < rotateTime)
        {
            t += Time.deltaTime;
            cameraPivot.rotation = Quaternion.Slerp(targetRot, startRot, t / rotateTime);
            yield return null;
        }
        cameraPivot.rotation = startRot;

        unstableCoroutine = null;
        Debug.Log("Niestabilna potka skończyła się.");
    }
    private IEnumerator FadeAudio(AudioSource src, float target, float time)
    {
        if (!src || time <= 0f)
        {
            if (src) src.volume = target;
            yield break;
        }

        float start = src.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, target, t / time);
            yield return null;
        }
        src.volume = target;
    }
    private IEnumerator FurryRoutine(float duration)
    {
        if (CursorManager.Instance == null)
        {
            Debug.LogWarning("FurryPotion: Brak CursorManager.Instance");
            yield break;
        }

        CursorManager.Instance.SetFurryCursor();

        yield return new WaitForSeconds(duration);

        CursorManager.Instance.SetDefaultCursor();
        furryCoroutine = null;
    }

    public void ExecuteEffect(PotionEffect effect)
    {
        // Central router: maps enum to concrete methods.
        switch (effect)
        {
            case PotionEffect.Blind: blindPotion(); break;
            case PotionEffect.Reggae: reggaePotion(); break;
            case PotionEffect.Metal: metalPotion(); break;
            case PotionEffect.Photophobia: photofobiaPotion(); break;
            case PotionEffect.Unstable: UnstablePotion(); break;
            case PotionEffect.Homeless: HomlessPotion(); break;
            case PotionEffect.Antidote: AntidotePotion(); break;

            case PotionEffect.Weaken1: WeakenPotionTier1(); break;
            case PotionEffect.Weaken2: WeakenPotionTier2(); break;
            case PotionEffect.Weaken3: WeakenPotionTier3(); break;

            case PotionEffect.BoostIdle1: BoostIdlePotionTier1(); break;
            case PotionEffect.BoostIdle2: BoostIdlePotionTier2(); break;
            case PotionEffect.BoostIdle3: BoostIdlePotionTier3(); break;

            case PotionEffect.ClickBoost1: ClickBoostPotionTier1(); break;
            case PotionEffect.ClickBoost2: ClickBoostPotionTier2(); break;
            case PotionEffect.ClickBoost3: ClickBoostPotionTier3(); break;

            case PotionEffect.Chaos: ChaosPotion(); break;
            case PotionEffect.Furry: FurryPotion(); break;

            case PotionEffect.None:
            default:
                Debug.Log("ExecuteEffect: no effect or not mapped.");
                break;
        }
    }


}

