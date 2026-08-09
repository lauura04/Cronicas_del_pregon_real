using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        if (puzzlePanel == null ||
            questionText == null ||
            answerContainer == null ||
            answerButtonPrefab == null)
        {
            Debug.LogError(
                "QuestionPuzzleManager no tiene todas las referencias UI asignadas."
            );
            return;
        }

        currentPuzzle = puzzle;
        HUDManager.Instance?.HideHUD();


        puzzlePanel.SetActive(true);

        questionText.text =
            currentPuzzle.QuestionData.Question;

        CreateAnswerButtons();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void CreateAnswerButtons()
    {
        ClearAnswerButtons();

        int answerCount =
            currentPuzzle.QuestionData.Answers.Count;

        Debug.Log(
            $"Creando {answerCount} botones de respuesta."
        );

        for (int i = 0; i < answerCount; i++)
        {
            int answerIndex = i;

            GameObject buttonObject = Instantiate(
                answerButtonPrefab,
                answerContainer,
                false
            );

            Button button =
                buttonObject.GetComponent<Button>();

            TMP_Text buttonText =
                buttonObject.GetComponentInChildren<TMP_Text>();

            if (button == null)
            {
                Debug.LogError(
                    $"El prefab {answerButtonPrefab.name} " +
                    "no tiene un componente Button en el objeto raíz."
                );

                Destroy(buttonObject);
                continue;
            }

            if (buttonText == null)
            {
                Debug.LogError(
                    $"El prefab {answerButtonPrefab.name} " +
                    "no contiene ningún TMP_Text."
                );

                Destroy(buttonObject);
                continue;
            }

            buttonText.text =
                currentPuzzle
                    .QuestionData
                    .Answers[answerIndex]
                    .answerText;

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
    if (currentPuzzle == null)
    {
        return;
    }

    
    QuestionPuzzle puzzle = currentPuzzle;

    bool correct = puzzle.CheckAnswer(answerIndex);

    ClosePuzzle();

    if (!correct)
    {
        puzzle.HandleWrongAnswer();
    }

     puzzle.HandleCorrectAnswer();
}

    public void ClosePuzzle()
    {
        currentPuzzle = null;

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
}