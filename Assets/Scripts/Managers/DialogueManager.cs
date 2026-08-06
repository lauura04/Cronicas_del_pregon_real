using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }


    [Header("Configuración")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Interfaz de diálogo")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Image characterPortraitImage;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    public bool IsDialogueActive =>
        currentDialogue != null;
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
    System.Action onFinished = null)
    {
        if (dialogue == null || dialogue.Lines == null ||
            dialogue.Lines.Count == 0)
        {
            Debug.LogError(
                "El diálogo está vacío o no ha sido asignado."
            );
            return;
        }

        if (dialogueCanvas == null ||
            dialogueText == null ||
            characterNameText == null ||
            characterPortraitImage == null)
        {
            Debug.LogError(
                "La interfaz de diálogo no está completamente asignada."
            );
            return;
        }

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }

        dialogueCanvas.SetActive(true);
        currentDialogue = dialogue;
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
        onDialogueFinished = null;
        currentLineText = null;
        currentLineIndex = 0;
        isTyping = false;

        dialogueCanvas.SetActive(false);

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }

        finishedAction?.Invoke();
    }
}