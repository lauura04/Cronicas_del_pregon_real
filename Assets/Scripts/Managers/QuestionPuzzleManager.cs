using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestionPuzzleManager : MonoBehaviour
{
    public static QuestionPuzzleManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TMP_Text questionText;

    [Header("Answers")]
    [SerializeField] private RectTransform answerContainer;
    [SerializeField] private GameObject answerButtonPrefab;

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

        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
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

        if(!CheckUIReferences())
            return;

        currentPuzzle = puzzle;
        currentQuestionData = puzzle.QuestionData;
        onAnswerSelected = null;

        OpenUI();
        ShowQuestion(currentQuestionData);
    }

    public void OpenSequentialQuestion(QuestionData questionData, Action<int> answerCallback)
    {
        if(questionData == null)
        {
            Debug.LogError("QuestionData es null");
            return;
        }
        if (!CheckUIReferences())
        {
            return;
        }

        currentPuzzle = null;
        currentQuestionData = questionData;
        onAnswerSelected = answerCallback;

        OpenUI();
        ShowQuestion(currentQuestionData);
    }

    public void RefreshQuestion(QuestionData questionData)
    {
        if(questionData == null)
        {
            Debug.LogError("QuestionData es null");
        }

        currentQuestionData = questionData;
        ShowQuestion(currentQuestionData);
    }
    private void OpenUI()
    {
        HUDManager.Instance?.HideHUD();
        puzzlePanel.SetActive(true);
        if(PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void ShowQuestion(QuestionData questionData)
{
    Debug.Log(
        $"Mostrando pregunta: {(questionData != null ? questionData.name : "NULL")}"
    );

    questionText.text = questionData.Question;

    CreateAnswerButtons(questionData);
}
    private void CreateAnswerButtons(QuestionData questionData)
{
    ClearAnswerButtons();

    if (questionData == null)
    {
        Debug.LogError("CreateAnswerButtons ha recibido un QuestionData null.");
        return;
    }

    if (questionData.Answers == null)
    {
        Debug.LogError(
            $"La pregunta '{questionData.name}' tiene la lista Answers en null."
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

    int answerCount = questionData.Answers.Count;

    Debug.Log(
        $"Creando {answerCount} botones para la pregunta {questionData.name}."
    );

    for (int i = 0; i < answerCount; i++)
    {
        int answerIndex = i;

        GameObject buttonObject = Instantiate(
            answerButtonPrefab,
            answerContainer,
            false
        );

        if (buttonObject == null)
        {
            Debug.LogError("No se ha podido instanciar el botón.");
            continue;
        }

        Button button = buttonObject.GetComponent<Button>();

        TMP_Text buttonText =
            buttonObject.GetComponentInChildren<TMP_Text>();

        if (button == null)
        {
            Debug.LogError(
                $"El prefab {answerButtonPrefab.name} no tiene Button."
            );

            Destroy(buttonObject);
            continue;
        }

        if (buttonText == null)
        {
            Debug.LogError(
                $"El prefab {answerButtonPrefab.name} no tiene TMP_Text."
            );

            Destroy(buttonObject);
            continue;
        }

        AnswerOption answer =
            questionData.Answers[answerIndex];

        if (answer == null)
        {
            Debug.LogError(
                $"La respuesta {answerIndex} de {questionData.name} es null."
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
        answerContainer
    );
}

 private void SelectAnswer(int answerIndex)
{
        //puzzle secuencial
        if (onAnswerSelected != null)
        {
            onAnswerSelected.Invoke(answerIndex);
            return;
        }
        //puzzle unico
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
        currentPuzzle = null;
        currentQuestionData = null;
        onAnswerSelected = null;

        ClearAnswerButtons();

        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
        HUDManager.Instance?.ShowHUD();


        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }

    private void ClearAnswerButtons()
    {
        if (answerContainer == null)
        {
            return;
        }

        for (int i = answerContainer.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(
                answerContainer.GetChild(i).gameObject
            );
        }
    }

    private bool CheckUIReferences()
    {
        if(puzzlePanel==null||questionText==null||answerContainer == null|| answerButtonPrefab == null)
        {
            Debug.LogError("QuestionPuzzleManager no tiene todas las referencias UI asignadas");
            return false;
        }

        return true;
    }
}