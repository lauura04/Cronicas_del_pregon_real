using System;
using UnityEngine;
using UnityEngine.Events;
public class SequentialQuestionsPuzzle : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private QuestionData[] questions;

    [Header("Dialogues")]
    [SerializeField] private DialogueData failDialogue;
    [SerializeField] private DialogueData successDialogue;

    [Header("Settings")]
    [SerializeField] private bool canRepeat = false;

    [Header("Events")]
    [SerializeField] private UnityEvent onPuzzleCompleted;
    [SerializeField] private UnityEvent onPuzzleFailed;

    private int currentQuestionIndex = 0;
    private bool completed = false;

    public void StartPuzzle()
    {
        if(completed && !canRepeat)
        {
            return;
        }
       if(questions == null || questions.Length == 0)
        {
            Debug.LogError($"{gameObject.name} no tiene preguntas asignadas");
            return;
        }

        currentQuestionIndex = 0;
        QuestionPuzzleManager.Instance.OpenSequentialQuestion(questions[currentQuestionIndex],OnAnswerSelected);
    }


    private void OnAnswerSelected(int answerIndex)
    {
        QuestionData currentQuestion = questions[currentQuestionIndex];

        if(answerIndex <0 || answerIndex >= currentQuestion.Answers.Count)
            return;
        AnswerOption selectedAnswer = currentQuestion.Answers[answerIndex];

        if (selectedAnswer.isCorrect)
        {
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer();
        }
    }

    private void HandleCorrectAnswer()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Length)
        {
            CompletePuzzle();
            return;
        }

        QuestionPuzzleManager.Instance.RefreshQuestion(questions[currentQuestionIndex]);
    }

    private void HandleWrongAnswer()
    {
        currentQuestionIndex = 0;
        QuestionPuzzleManager.Instance.ClosePuzzle();

       
        onPuzzleFailed?.Invoke();
        if (failDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(failDialogue);
        }
    }

    private void CompletePuzzle()
    {
        completed = true;
        QuestionPuzzleManager.Instance.ClosePuzzle();

        if (successDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(successDialogue);
            DialogueManager.Instance.onDialogueFinished = OnSuccessDialogueFinish;
        }
        else
        {
            FinishPuzzle();
        }
    }

    private void OnSuccessDialogueFinish()
    {
        DialogueManager.Instance.onDialogueFinished = null;
        FinishPuzzle();
    }

    private void FinishPuzzle()
    {
        onPuzzleCompleted?.Invoke();
    }
}
