using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animator : MonoBehaviour
{
    private Dictionary<RectTransform, Coroutine> activeMoveUICoroutines = new Dictionary<RectTransform, Coroutine>();
    private Dictionary<RectTransform, Coroutine> activeScaleUICoroutines = new Dictionary<RectTransform, Coroutine>();
    private Dictionary<Image, Coroutine> activeFadeUICoroutines = new Dictionary<Image, Coroutine>();

    public void ChangePosition(Transform transform, Vector3 basePosition, Vector3 offset, float duration = .1f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        if (activeMoveUICoroutines.TryGetValue(rectTransform, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            activeMoveUICoroutines.Remove(rectTransform);
        }

        Vector3 targetPosition = basePosition + offset;
        Coroutine newCoroutine = StartCoroutine(CoChangePosition(rectTransform, targetPosition, duration));
        activeMoveUICoroutines[rectTransform] = newCoroutine;
    }

    private IEnumerator CoChangePosition(RectTransform rectTransform, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 initialPosition = rectTransform.anchoredPosition;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;

        if (activeMoveUICoroutines.ContainsKey(rectTransform))
        {
            activeMoveUICoroutines.Remove(rectTransform);
        }
    }

    public void ChangeScale(Transform transform, float targetScale, float duration = 0.25f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        if (activeScaleUICoroutines.TryGetValue(rectTransform, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            activeScaleUICoroutines.Remove(rectTransform);
        }

        Coroutine newCoroutine = StartCoroutine(CoChangeScale(rectTransform, targetScale, duration));
        activeScaleUICoroutines[rectTransform] = newCoroutine;
    }

    private IEnumerator CoChangeScale(RectTransform rectTransform, float newScale, float duration)
    {
        float time = 0;
        Vector3 initialScale = rectTransform.localScale;
        Vector3 targetScale = new Vector3(newScale, newScale, newScale);

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;

        if (activeScaleUICoroutines.ContainsKey(rectTransform))
        {
            activeScaleUICoroutines.Remove(rectTransform);
        }
    }

    public void FadeImage(Image image, float targetAlpha, float duration = 0.25f)
    {
        if (activeFadeUICoroutines.TryGetValue(image, out Coroutine runningCoroutine))
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            activeFadeUICoroutines.Remove(image);
        }

        Coroutine newCoroutine = StartCoroutine(CoFadeImage(image, targetAlpha, duration));
        activeFadeUICoroutines[image] = newCoroutine;
    }

    private IEnumerator CoFadeImage(Image image, float targetAlpha, float duration)
    {
        float time = 0f;
        Color currentColor = image.color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(currentColor.r, currentColor.b, currentColor.g, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        if (activeFadeUICoroutines.ContainsKey(image))
        {
            activeFadeUICoroutines.Remove(image);
        }
    }
}