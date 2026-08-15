using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{

    private ChapterIntroData introData;
    private void Start()
    {
        introData = ChapterManager.Instance.GetCurrentIntro();

        if(introData == null)
        {
            Debug.LogError("No se ha encontrado la introducción del capítulo");
            return;
        }

        DialogueManager.Instance.StartDialogue(introData.Dialogue, OnIntroFinished);
    }

    private void OnIntroFinished()
    {
        SceneLoader.Instance.LoadScene(introData.GameplayScene);
    }
    /*public static IntroController Instance { get; private set; }

    [SerializeField] private DialogueData Introduccion;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {

        DialogueManager.Instance.StartDialogue(
            Introduccion, FinishIntro);
    }


    private void FinishIntro()
    {
        SceneLoader.Instance.LoadScene("Tutorial");
    }*/
}