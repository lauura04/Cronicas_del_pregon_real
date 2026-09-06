using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{

    private ChapterIntroData introData;

    [SerializeField] private AudioClip introClip;
    [SerializeField] private Image backgroundImage;
    private void Start()
    {
        introData = ChapterManager.Instance.GetCurrentIntro();

        if (introData == null)
        {
            Debug.LogError("No se ha encontrado la introducción del capítulo");
            return;
        }
        if (backgroundImage != null && introData.Background != null)
        {
            backgroundImage.sprite = introData.Background;
        }
        MusicManager.Instance.PlayMusic(introClip);
        DialogueManager.Instance.SetUIType(DialogueUIType.Intro);

        DialogueManager.Instance.StartDialogue(
            introData.Dialogue,
            OnIntroFinished
        );

    }

    private void OnIntroFinished()
    {
        StartCoroutine(FinishIntro());
    }

    private IEnumerator FinishIntro()
    {
         DialogueManager.Instance.SetUIType(DialogueUIType.Gameplay);
  yield return ScreenFade.Instance.FadeOutCoroutine();
        SceneLoader.Instance.LoadScene(introData.GameplayScene);
    }
   
}