using UnityEngine;
using UnityEngine.Events;

public class QuestionPuzzle : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] private QuestionData questionData;

    [Header("Wrong Answer")]
    [SerializeField] private DialogueData wrongAnswerDialogue;
    [Header("Correct Answer")]
    [SerializeField] private DialogueData correctAnswerDialogue;

    [Header("Events")]
    [SerializeField] private UnityEvent onCorrectAnswer;
    [SerializeField] private UnityEvent onWrongAnswer;

    private bool isCompleted;

    public QuestionData QuestionData => questionData;
    public bool IsCompleted => isCompleted;

    public void Open()
    {
        if (isCompleted)
        {
            return;
        }

        if (QuestionPuzzleManager.Instance == null)
        {
            Debug.LogError(
                "QuestionPuzzleManager.Instance no existe."
            );
            return;
        }

        QuestionPuzzleManager.Instance.OpenPuzzle(this);
    }

    public bool CheckAnswer(int answerIndex)
    {
        if (isCompleted)
        {
            return true;
        }

        if (questionData == null)
        {
            Debug.LogError(
                $"No hay QuestionData asignado en {gameObject.name}."
            );
            return false;
        }

        if (answerIndex < 0 ||
            answerIndex >= questionData.Answers.Count)
        {
            Debug.LogError(
                $"Índice de respuesta inválido: {answerIndex}"
            );
            return false;
        }

        AnswerOption selectedAnswer =
            questionData.Answers[answerIndex];

        if (!selectedAnswer.isCorrect)
        {
            return false;
        }


        isCompleted = true;

        Debug.Log("Respuesta correcta.");

        return true;
    }
    public void HandleCorrectAnswer()
    {
        onCorrectAnswer?.Invoke();
        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "DialogueManager.Instance no existe."
            );
            return;
        }
        DialogueManager.Instance.StartDialogue(
            correctAnswerDialogue
        );

    }
    public void HandleWrongAnswer()
    {
        Debug.Log("Respuesta incorrecta.");

        onWrongAnswer?.Invoke();

        if (wrongAnswerDialogue == null)
        {
            Debug.LogWarning(
                $"No hay Wrong Answer Dialogue asignado en {gameObject.name}."
            );
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "DialogueManager.Instance no existe."
            );
            return;
        }

        DialogueManager.Instance.StartDialogue(
            wrongAnswerDialogue
        );
    }

    public void ResetPuzzle()
    {
        isCompleted = false;
    }
}