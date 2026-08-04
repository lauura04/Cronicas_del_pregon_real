using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private float typingSpeed = 0.05f;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    private TMP_Text dialogueText;
    private TMP_Text characterNameText;
    private Image characterPortraitImage;

    private System.Action onDialogueFinished;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLineText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(
        DialogueData dialogue,
        TMP_Text textComponent,
        TMP_Text nameTextComponent,
        Image portraitImageComponent,
        System.Action onFinished)
    {
        if (dialogue == null || dialogue.Lines.Count == 0)
        {
            Debug.LogError("El diálogo está vacío o no ha sido asignado.");
            return;
        }

        if (textComponent == null)
        {
            Debug.LogError("No se ha asignado el texto del diálogo.");
            return;
        }

        if (nameTextComponent == null)
        {
            Debug.LogError("No se ha asignado el texto del nombre.");
            return;
        }

        if (portraitImageComponent == null)
        {
            Debug.LogError("No se ha asignado la imagen del retrato.");
            return;
        }

        currentDialogue = dialogue;
        dialogueText = textComponent;
        characterNameText = nameTextComponent;
        characterPortraitImage = portraitImageComponent;
        onDialogueFinished = onFinished;
        currentLineIndex = 0;

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue.Lines[currentLineIndex];

        currentLineText = line.Text;

        if (line.Character != null)
        {
            characterNameText.text = line.Character.CharacterName;

            Sprite portrait = line.Character.Portrait;
            characterPortraitImage.sprite = portrait;
            characterPortraitImage.gameObject.SetActive(portrait != null);
        }
        else
        {
            characterNameText.text = "";
            characterPortraitImage.sprite = null;
            characterPortraitImage.gameObject.SetActive(false);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(
            TypeWriterEffect(currentLineText)
        );
    }

    public void NextLine()
    {
        if (isTyping)
        {
            CompleteCurrentLine();
            return;
        }

        currentLineIndex++;

        if (currentLineIndex < currentDialogue.Lines.Count)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeWriterEffect(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char character in text)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentLineText;
        isTyping = false;
    }

    private void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = "";
        characterNameText.text = "";

        characterPortraitImage.sprite = null;
        characterPortraitImage.gameObject.SetActive(false);

        System.Action finishedAction = onDialogueFinished;

        currentDialogue = null;
        dialogueText = null;
        characterNameText = null;
        characterPortraitImage = null;
        onDialogueFinished = null;
        currentLineText = null;
        currentLineIndex = 0;
        isTyping = false;

        finishedAction?.Invoke();
    }
}