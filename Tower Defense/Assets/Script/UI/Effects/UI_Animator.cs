using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Animator : MonoBehaviour
{
    private Dictionary<RectTransform, Coroutine> activeMoveUICoroutines = new Dictionary<RectTransform, Coroutine>();

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
}