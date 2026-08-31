using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class LetterPuzzleManager : MonoBehaviour
{
    public static LetterPuzzleManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private TMP_Text letterSlotPrefab;
    [SerializeField] private Image letterBackground;
    [SerializeField] private int charactersPerLine = 29;

    [Header("Puzzle")]
    private LetterPuzzleData currentPuzzle;

    [Header("Events")]
    [SerializeField] private UnityEvent onCorrect;
    [SerializeField] private UnityEvent onIncorrect;
    [SerializeField] private DialogueData wrongAnswer;

    [SerializeField] private AudioClip letterAudio;
    private List<TMP_Text> letterSlots = new List<TMP_Text>();

    private StringBuilder currentAnswer;

    private int currentPosition;
    private bool isPlaying;
    public bool IsPlaying => isPlaying;

    private Action onPuzzleCorrect;
    

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

    private void Start()
    {
        puzzlePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isPlaying)
            return;
        HandleInput();
    }

    public void StartPuzzle(LetterPuzzleData puzzleData, Action onCorrect = null)
    {
        if (puzzleData == null)
        {
            Debug.LogError("No se ha asignado ningún LetterPuzzleData")
;            return;
        }
        currentPuzzle = puzzleData;
        isPlaying = true;
        currentPosition = 0;
        puzzlePanel.SetActive(true);
        onPuzzleCorrect = onCorrect;

        letterBackground.sprite = currentPuzzle.letterBackground;

        CreatePuzzle();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void CreatePuzzle()
{
    foreach (Transform child in slotsParent)
    {
        Destroy(child.gameObject);
    }

    letterSlots.Clear();

    string correctText = currentPuzzle.correctText;

    currentAnswer = new StringBuilder(correctText.Length);

    int visualPosition = 0;

    for (int i = 0; i < correctText.Length; i++)
    {
        char character = correctText[i];

        // Si estamos al principio de una palabra,
        // comprobamos si cabe entera en esta línea.
        if (char.IsLetter(character) &&
            (i == 0 || !char.IsLetter(correctText[i - 1])))
        {
            int wordLength = GetWordLength(correctText, i);

            int remainingSpace =
                charactersPerLine - (visualPosition % charactersPerLine);

            if (wordLength > remainingSpace &&
                remainingSpace != charactersPerLine)
            {
                // Rellenamos lo que queda de línea
                // para que la palabra pase entera abajo.
                for (int j = 0; j < remainingSpace; j++)
                {
                    TMP_Text filler = Instantiate(
                        letterSlotPrefab,
                        slotsParent
                    );

                    filler.text = "";
                    visualPosition++;
                }
            }
        }

        TMP_Text slot = Instantiate(
            letterSlotPrefab,
            slotsParent
        );

        letterSlots.Add(slot);

        if (char.IsLetter(character))
        {
            slot.text = "_";
            currentAnswer.Append('_');
        }
        else
        {
            slot.text = character.ToString();
            currentAnswer.Append(character);
        }

        visualPosition++;
    }

    MoveToNextLetter();
}

private int GetWordLength(string text, int startIndex)
    {
        int length = 0;
        for(int i = startIndex; i<text.Length && char.IsLetter(text[i]);i++){
         length++;   
        }
        return length;
    }

    private void HandleInput()
    {
        foreach (char input in Input.inputString)
        {
            if (char.IsLetter(input))
            {
                AddLetter(char.ToUpper(input));
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveLetter();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    private void AddLetter(char letter)
    {
        string correctText = currentPuzzle.correctText;
        if (currentPosition >= correctText.Length)
        {
            return;
        }

        if (!char.IsLetter(correctText[currentPosition]))
        {
            MoveToNextLetter();
        }

        if (currentPosition >= correctText.Length)
        {
            return;
        }

        currentAnswer[currentPosition] = letter;

        letterSlots[currentPosition].text = letter.ToString();

        currentPosition++;
        MoveToNextLetter();

        if (currentPosition >= correctText.Length)
        {
            CheckAnswer();
        }
    }

    private void MoveToNextLetter()
    {
        string correctText = currentPuzzle.correctText;

        while (currentPosition < correctText.Length && !char.IsLetter(correctText[currentPosition]))
        {
            currentPosition++;
        }
    }

    private void RemoveLetter()
    {
        string correctText = currentPuzzle.correctText;
        if (currentPosition <= 0)
        {
            return;
        }

        currentPosition--;
        while (currentPosition >= 0 && !char.IsLetter(correctText[currentPosition]))
        {
            currentPosition--;
        }

        if (currentPosition < 0)
        {
            currentPosition = 0;
            MoveToNextLetter();
            return;
        }

        currentAnswer[currentPosition] = '_';
        letterSlots[currentPosition].text = "_";
    }

    private void CheckAnswer()
    {
        string answer = currentAnswer.ToString();
        string correctText = currentPuzzle.correctText;

        if (answer.Equals(correctText, System.StringComparison.OrdinalIgnoreCase))
        {
            CompletePuzzle();
        }
        else
        {
            IncorrectAnswer();
        }
    }

    private void IncorrectAnswer()
    {
        Debug.Log("La frase no es correcta");
        onIncorrect?.Invoke();
        puzzlePanel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
        if(DialogueManager.Instance == null)
        {
            return;
        }
        DialogueManager.Instance.StartDialogue(wrongAnswer);
    }

    private void CompletePuzzle()
    {
        SFXManager.Instance.PlaySFX(letterAudio);
        isPlaying = false;
        puzzlePanel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }

        onCorrect?.Invoke();
        onPuzzleCorrect?.Invoke();

        onPuzzleCorrect=null;
    }

    public void Close()
    {
        isPlaying = true;
        puzzlePanel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }
}
