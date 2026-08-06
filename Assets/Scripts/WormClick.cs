using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class WormClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float disappearDuration = 0.4f;
    [SerializeField] private bool shrinkOnDisappear = true;

    private SoundManager soundManager;
    private SpriteRenderer spriteRenderer;
    private Graphic uiGraphic;
    private Collider2D col;

    private bool isDisappearing = false;
    private Vector3 startScale;

    private void Awake()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        uiGraphic = GetComponent<Graphic>(); // np. Image w UI
        col = GetComponent<Collider2D>();

        startScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SquashWorm();
    }

    public void SquashWorm()
    {
        if (isDisappearing)
            return;

        Debug.Log("WORM CLICKED: " + gameObject.name);

        if (soundManager != null)
        {
            soundManager.WormSquishSound();
        }

        StartCoroutine(DisappearSlowly());
    }

    private IEnumerator DisappearSlowly()
    {
        isDisappearing = true;

        if (col != null)
            col.enabled = false;

        if (uiGraphic != null)
            uiGraphic.raycastTarget = false;

        Color startColor = Color.white;

        if (spriteRenderer != null)
            startColor = spriteRenderer.color;
        else if (uiGraphic != null)
            startColor = uiGraphic.color;

        float timer = 0f;

        while (timer < disappearDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / disappearDuration;

            float alpha = Mathf.Lerp(startColor.a, 0f, progress);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(
                    startColor.r,
                    startColor.g,
                    startColor.b,
                    alpha
                );
            }

            if (uiGraphic != null)
            {
                uiGraphic.color = new Color(
                    startColor.r,
                    startColor.g,
                    startColor.b,
                    alpha
                );
            }

            if (shrinkOnDisappear)
            {
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}