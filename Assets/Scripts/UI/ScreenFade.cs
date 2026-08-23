using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
   public static ScreenFade Instance {get; private set;}

   [SerializeField] private CanvasGroup fadeCanvasGroup;
   [SerializeField] private float fadeDuration = 1f;

   private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0f,1f));
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(1f,0f));
    }
    public void FadeOutAndIn(float blackDuration = 1f)
    {
        StartCoroutine(FadeSequence(blackDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        fadeCanvasGroup.blocksRaycasts = true;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime/fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
        if (endAlpha == 0f)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    private IEnumerator FadeSequence(float blackDuration)
    {
        yield return Fade(0f,1f);

        yield return new WaitForSeconds(blackDuration);

        yield return Fade(1f,0f);
    }

    public IEnumerator FadeOutCoroutine()
    {
        yield return Fade(0f,1f);
    }
}
