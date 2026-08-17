using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text;

public class LetterPuzzleManager : MonoBehaviour
{
    public static LetterPuzzleManager Instance{get;private set;}

    [Header("UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private TMP_Text letterSlotPrefab;

    [Header("Puzzle")]
    [TextArea]
    [SerializeField] private string correctText = "LA REINA LLEGARA MAÑANA";

    [Header("Events")]
    [SerializeField] private UnityEvent onCorrect;
    [SerializeField] private UnityEvent onIncorrect;

    private List<TMP_Text> letterSlots = new List<TMP_Text>();

    private StringBuilder currentAnswer;

    private int currentPosition;
    private bool isPlaying;

    private void Awake()
    {
        if(Instance!=null && Instance != this)
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
        if(!isPlaying)
            return;
        HandleInput();
    }
    
    public void StartPuzzle()
    {
        isPlaying = true;
        currentPosition = 0;
        puzzlePanel.SetActive(true);

        CreatePuzzle();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void CreatePuzzle()
    {
        foreach(Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        letterSlots.Clear();

        currentAnswer = new StringBuilder(correctText.Length);

        for(int i = 0; i<correctText.Length; i++)
        {
            char character = correctText[i];

            TMP_Text slot = Instantiate(letterSlotPrefab, slotsParent);

            letterSlots.Add(slot);

            if (char.IsLetter(character))
            {
                slot.text="_";
                currentAnswer.Append('_');
            }

            else
            {
                slot.text = character.ToString();
                currentAnswer.Append(character);
            }
        }

        MoveToNextLetter();
    }

    private void HandleInput()
    {
        foreach(char input in Input.inputString)
        {
            if (char.IsLetter(input))
            {
                AddLetter(char.ToUpper(input));
            }
        }

        /*if (HandleInput.GetKeyDown(KeyCode.Backspace))
        {
            Remove.Letter();
        }

        if (HandleInput.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }*/
    }

    private void AddLetter(char letter)
    {
        if(currentPosition >= correctText.Length)
        {
            return;
        }

        if (!char.IsLetter(correctText[currentPosition]))
        {
            MoveToNextLetter();
        }

        if(currentPosition >= correctText.Length)
        {
            return;
        }

        currentAnswer[currentPosition] = letter;

        letterSlots[currentPosition].text = letter.ToString();

        currentPosition++;
        MoveToNextLetter();
    }

    private void MoveToNextLetter()
    {
        while(currentPosition<correctText.Length && !char.IsLetter(correctText[currentPosition]))
        {
            currentPosition++;
        }
    }
    
    private void RemoveLetter()
    {
        if (currentPosition <= 0)
        {
            return;
        }

        currentPosition--;
        while(currentPosition>=0 && !char.IsLetter(correctText[currentPosition]))
        {
            currentPosition--;
        }

        if(currentPosition < 0)
        {
            currentPosition = 0;
            MoveToNextLetter();
            return;
        }

        currentAnswer[currentPosition]='_';
        letterSlots[currentPosition].text = "_";
    }

    private void CheckAnswer()
    {
        string answer = currentAnswer.ToString();

        if(answer.Equals(correctText, System.StringComparison.OrdinalIgnoreCase))
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
    }    

    private void CompletePuzzle()
    {
        isPlaying = false;
        puzzlePanel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }

        onCorrect?.Invoke();
    }
}
