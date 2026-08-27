using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueUIType
{
    Gameplay,
    Intro
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }


    [Header("Configuración")]
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("GamePlay UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private Image leftDialogueImage;
    [SerializeField] private Image rightDialogueImage;

    [Header("Intro UI")]
    [SerializeField] private GameObject introDialoguePanel;
    [SerializeField] private TMP_Text introDialogueText;
    [SerializeField] private TMP_Text introCharacterNameText;
    [SerializeField] private Image introCharacterPortraitImage;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    public bool IsDialogueActive =>
        currentDialogue != null;
    public System.Action onDialogueFinished;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLineText;

    private DialogueUIType currentUIType = DialogueUIType.Gameplay;

    private GameObject currentPanel;
    private TMP_Text currentDialogueTextUI;
    private TMP_Text currentCharacterNameTextUI;
    private Image currentCharacterPortraitImageUI;

    public void SetUIType(DialogueUIType type)
    {
        currentUIType = type;
    }

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
    private void SelectCurrentUI()
    {
        if (currentUIType == DialogueUIType.Intro)
        {
            currentPanel = introDialoguePanel;
            currentDialogueTextUI = introDialogueText;
            currentCharacterNameTextUI = introCharacterNameText;
            currentCharacterPortraitImageUI = introCharacterPortraitImage;
        }
        else
        {
            currentPanel = dialoguePanel;
            currentDialogueTextUI = dialogueText;
            currentCharacterNameTextUI = characterNameText;
            currentCharacterPortraitImageUI = characterPortraitImage;
        }
    }
    public void StartDialogue(
    DialogueData dialogue,
    System.Action onFinished = null)
    {
        SelectCurrentUI();

        if (dialogue == null || dialogue.Lines == null ||
            dialogue.Lines.Count == 0)
        {
            Debug.LogError(
                "El diálogo está vacío o no ha sido asignado."
            );
            return;
        }

        if (currentPanel == null ||
            currentDialogueTextUI == null ||
            currentCharacterNameTextUI == null ||
            currentCharacterPortraitImageUI == null)
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

        HUDManager.Instance?.HideHUD();

        currentPanel.SetActive(true);

        currentDialogue = dialogue;
        onDialogueFinished = onFinished;
        currentLineIndex = 0;

        ShowCurrentLine();
    }
    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue.Lines[currentLineIndex];

        currentLineText = line.Text;
        UpdateDialogueImages(line);

        if (line.Character != null)
        {
            currentCharacterNameTextUI.text =
                line.Character.CharacterName;

            Sprite portrait = line.Character.Portrait;

            currentCharacterPortraitImageUI.sprite = portrait;

            currentCharacterPortraitImageUI.gameObject.SetActive(
                portrait != null
            );
        }
        else
        {
            currentCharacterNameTextUI.text = "";

            currentCharacterPortraitImageUI.sprite = null;

            currentCharacterPortraitImageUI.gameObject.SetActive(false);
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

        currentDialogueTextUI.text = "";

        foreach (char character in text)
        {
            currentDialogueTextUI.text += character;

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

        currentDialogueTextUI.text = currentLineText;

        isTyping = false;
    }

    private void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        currentDialogueTextUI.text = "";
        currentCharacterNameTextUI.text = "";

        currentCharacterPortraitImageUI.sprite = null;

        currentCharacterPortraitImageUI.gameObject.SetActive(false);
        SetDialogueImage(leftDialogueImage, null);
    SetDialogueImage(rightDialogueImage, null);

        System.Action finishedAction = onDialogueFinished;

        currentDialogue = null;
        onDialogueFinished = null;
        currentLineText = null;
        currentLineIndex = 0;
        isTyping = false;

        currentPanel.SetActive(false);

        HUDManager.Instance?.ShowHUD();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }

        finishedAction?.Invoke();
    }

    private void UpdateDialogueImages(DialogueLine line)
    {
        SetDialogueImage(leftDialogueImage,line.LeftImage);
        SetDialogueImage(rightDialogueImage, line.RightImage);

    }
    private void SetDialogueImage(Image image, Sprite sprite)
    {
        if (sprite != null)
        {
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }
        else
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }
    }
}