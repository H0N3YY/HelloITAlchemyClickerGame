using System.Collections;
using UnityEngine;

public class StirAnimation : MonoBehaviour
{
    [SerializeField] private float distance = 20f;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float duration = 1f;

    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void StartStirring()
    {
        Debug.Log("Start mieszania");

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(Stir());
    }

    private IEnumerator Stir()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Mathf.Sin(elapsedTime * speed) * distance;

            rectTransform.anchoredPosition =
                startPosition + new Vector2(offsetX, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = startPosition;
        animationCoroutine = null;
    }
}