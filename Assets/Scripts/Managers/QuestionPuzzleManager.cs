using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPuzzleManager : MonoBehaviour
{
    public static QuestionPuzzleManager Instance { get; private set; }

    [Header("Short Answers UI")]
    [SerializeField] private GameObject shortAnswersPanel;
    [SerializeField] private TMP_Text shortQuestionText;
    [SerializeField] private RectTransform shortAnswerContainer;
    [SerializeField] private GameObject shortAnswerButtonPrefab;

    [Header("Long Answers UI")]
    [SerializeField] private GameObject longAnswersPanel;
    [SerializeField] private TMP_Text longQuestionText;
    [SerializeField] private RectTransform longAnswerContainer;
    [SerializeField] private GameObject longAnswerButtonPrefab;

    private GameObject currentPanel;
    private TMP_Text currentQuestionText;
    private RectTransform currentAnswerContainer;
    private GameObject currentAnswerButtonPrefab;

    private QuestionPuzzle currentPuzzle;
    private QuestionData currentQuestionData;
    private Action<int> onAnswerSelected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (shortAnswersPanel != null)
        {
            shortAnswersPanel.SetActive(false);
        }

        if (longAnswersPanel != null)
        {
            longAnswersPanel.SetActive(false);
        }
    }

    public void OpenPuzzle(QuestionPuzzle puzzle)
    {
        if (puzzle == null)
        {
            Debug.LogError("QuestionPuzzle es null.");
            return;
        }

        if (puzzle.QuestionData == null)
        {
            Debug.LogError(
                $"El puzzle {puzzle.gameObject.name} no tiene QuestionData."
            );
            return;
        }

        currentPuzzle = puzzle;
        currentQuestionData = puzzle.QuestionData;
        onAnswerSelected = null;

        if (!SelectUI(currentQuestionData))
        {
            return;
        }

        OpenUI();
        ShowQuestion(currentQuestionData);
    }

    public void OpenSequentialQuestion(
        QuestionData questionData,
        Action<int> answerCallback
    )
    {
        if (questionData == null)
        {
            Debug.LogError("QuestionData es null.");
            return;
        }

        currentPuzzle = null;
        currentQuestionData = questionData;
        onAnswerSelected = answerCallback;

        if (!SelectUI(currentQuestionData))
        {
            return;
        }

        OpenUI();
        ShowQuestion(currentQuestionData);
    }

    public void RefreshQuestion(QuestionData questionData)
    {
        if (questionData == null)
        {
            Debug.LogError("QuestionData es null.");
            return;
        }

        ClearAnswerButtons();
        HideAllPanels();

        currentQuestionData = questionData;

        if (!SelectUI(currentQuestionData))
        {
            return;
        }

        currentPanel.SetActive(true);
        ShowQuestion(currentQuestionData);
    }

    private bool SelectUI(QuestionData questionData)
    {
        if (questionData == null)
        {
            return false;
        }

        switch (questionData.Layout)
        {
            case QuestionLayout.ShortAnswers:
                currentPanel = shortAnswersPanel;
                currentQuestionText = shortQuestionText;
                currentAnswerContainer = shortAnswerContainer;
                currentAnswerButtonPrefab = shortAnswerButtonPrefab;
                break;

            case QuestionLayout.LongAnswers:
                currentPanel = longAnswersPanel;
                currentQuestionText = longQuestionText;
                currentAnswerContainer = longAnswerContainer;
                currentAnswerButtonPrefab = longAnswerButtonPrefab;
                break;

            default:
                Debug.LogError(
                    $"Layout desconocido en la pregunta {questionData.name}."
                );
                return false;
        }

        return CheckCurrentUIReferences();
    }

    private void OpenUI()
    {
        HideAllPanels();

        HUDManager.Instance?.HideHUD();

        currentPanel.SetActive(true);

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void ShowQuestion(QuestionData questionData)
    {
        if (questionData == null)
        {
            Debug.LogError("ShowQuestion ha recibido un QuestionData null.");
            return;
        }

        Debug.Log($"Mostrando pregunta: {questionData.name}");

        currentQuestionText.text = questionData.Question;
        CreateAnswerButtons(questionData);
    }

    private void CreateAnswerButtons(QuestionData questionData)
    {
        ClearAnswerButtons();

        if (questionData == null)
        {
            Debug.LogError(
                "CreateAnswerButtons ha recibido un QuestionData null."
            );
            return;
        }

        if (questionData.Answers == null)
        {
            Debug.LogError(
                $"La pregunta '{questionData.name}' tiene Answers en null."
            );
            return;
        }

        if (questionData.Answers.Count == 0)
        {
            Debug.LogError(
                $"La pregunta '{questionData.name}' no tiene respuestas."
            );
            return;
        }

        for (int i = 0; i < questionData.Answers.Count; i++)
        {
            int answerIndex = i;
            AnswerOption answer = questionData.Answers[answerIndex];

            if (answer == null)
            {
                Debug.LogError(
                    $"La respuesta {answerIndex} de " +
                    $"{questionData.name} es null."
                );
                continue;
            }

            GameObject buttonObject = Instantiate(
                currentAnswerButtonPrefab,
                currentAnswerContainer,
                false
            );

            Button button = buttonObject.GetComponent<Button>();
            TMP_Text buttonText =
                buttonObject.GetComponentInChildren<TMP_Text>();

            if (button == null)
            {
                Debug.LogError(
                    $"El prefab {currentAnswerButtonPrefab.name} " +
                    "no tiene un componente Button."
                );

                Destroy(buttonObject);
                continue;
            }

            if (buttonText == null)
            {
                Debug.LogError(
                    $"El prefab {currentAnswerButtonPrefab.name} " +
                    "no tiene un componente TMP_Text."
                );

                Destroy(buttonObject);
                continue;
            }

            buttonText.text = answer.answerText;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(
                () => SelectAnswer(answerIndex)
            );
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            currentAnswerContainer
        );
    }

    private void SelectAnswer(int answerIndex)
    {
        // Puzzle secuencial
        if (onAnswerSelected != null)
        {
            onAnswerSelected.Invoke(answerIndex);
            return;
        }

        // Puzzle único
        if (currentPuzzle == null)
        {
            return;
        }

        QuestionPuzzle puzzle = currentPuzzle;
        bool correct = puzzle.CheckAnswer(answerIndex);

        ClosePuzzle();

        if (correct)
        {
            puzzle.HandleCorrectAnswer();
        }
        else
        {
            puzzle.HandleWrongAnswer();
        }
    }

    public void ClosePuzzle()
    {
        ClearAnswerButtons();
        HideAllPanels();

        currentPuzzle = null;
        currentQuestionData = null;
        onAnswerSelected = null;

        currentPanel = null;
        currentQuestionText = null;
        currentAnswerContainer = null;
        currentAnswerButtonPrefab = null;

        HUDManager.Instance?.ShowHUD();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }

    private void ClearAnswerButtons()
    {
        if (currentAnswerContainer == null)
        {
            return;
        }

        for (int i = currentAnswerContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(
                currentAnswerContainer.GetChild(i).gameObject
            );
        }
    }

    private void HideAllPanels()
    {
        if (shortAnswersPanel != null)
        {
            shortAnswersPanel.SetActive(false);
        }

        if (longAnswersPanel != null)
        {
            longAnswersPanel.SetActive(false);
        }
    }

    private bool CheckCurrentUIReferences()
    {
        if (currentPanel == null ||
            currentQuestionText == null ||
            currentAnswerContainer == null ||
            currentAnswerButtonPrefab == null)
        {
            Debug.LogError(
                $"Faltan referencias de la interfaz para " +
                $"{currentQuestionData.Layout}."
            );

            return false;
        }

        return true;
    }
}