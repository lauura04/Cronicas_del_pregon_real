using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public static IntroController Instance { get; private set; }

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
    }
}