using System;
using System.Collections.Generic;
using UnityEngine;


public enum QuestionLayout
{
    ShortAnswers,
    LongAnswers
}
[CreateAssetMenu(fileName = "NewQuestion", menuName = "Game/Question")]
public class QuestionData : ScriptableObject
{

    [Header("UI LAYOUT")]
    [SerializeField] private QuestionLayout questionLayout = QuestionLayout.ShortAnswers;

    [Header("Question")]
    [TextArea(2, 5)]
    [SerializeField] private string question;

    [SerializeField] private List<AnswerOption> answers = new List<AnswerOption>();
    public QuestionLayout Layout => questionLayout;
    public string Question => question;

    public IReadOnlyList<AnswerOption> Answers => answers;

}

[Serializable]
public class AnswerOption
{
    [TextArea(1, 3)]
    public string answerText;

    public bool isCorrect;
}