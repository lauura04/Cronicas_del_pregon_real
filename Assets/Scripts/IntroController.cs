using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Image characterPortraitImage;

    private void Start()
    {
        DialogueManager.Instance.StartDialogue(
            dialogue,
            dialogueText,
            characterNameText,
            characterPortraitImage,
            FinishIntro
        );
    }

    public void ContinueDialogue()
    {
        DialogueManager.Instance.NextLine();
    }

    private void FinishIntro()
    {
        SceneLoader.Instance.LoadScene("Tutorial");
    }
}