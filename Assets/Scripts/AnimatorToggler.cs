using UnityEngine;

public class AnimatorToggler : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator;

    [Header("State names")]
    [SerializeField] private string burnStateName = "Burn";
    [SerializeField] private string idleStateName = "Idle";

    [Header("Playback")]
    [SerializeField, Tooltip("Czas crossfade przy włączaniu Burn")]
    private float crossFadeDuration = 0.1f;

    private int burnHash;
    private int idleHash;

    private void Awake()
    {
        if (targetAnimator == null)
            Debug.LogWarning("Brak przypiętego Animatora.", this);

        burnHash = Animator.StringToHash(burnStateName);
        idleHash = Animator.StringToHash(idleStateName);
    }

    public void EnableAnimator()
    {
        if (!targetAnimator) return;


        if (!targetAnimator.enabled) targetAnimator.enabled = true;

        targetAnimator.CrossFade(burnHash, crossFadeDuration, 0, 0f);

    }

    public void DisableAnimator()
    {
        if (!targetAnimator) return;


        bool wasEnabled = targetAnimator.enabled;
        targetAnimator.enabled = true;
        targetAnimator.Play(idleHash, 0, 0f);
        targetAnimator.Update(0f);
        targetAnimator.enabled = false;
    }

    public void ToggleAnimator()
    {
        if (!targetAnimator) return;

        if (!targetAnimator.enabled)
        {
            EnableAnimator();
        }
        else
        {
            DisableAnimator();
        }
    }


}
