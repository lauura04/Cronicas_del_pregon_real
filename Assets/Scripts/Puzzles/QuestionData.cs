using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Game/Question")]
public class QuestionData : ScriptableObject
{
    [TextArea(2, 5)]
    [SerializeField] private string question;

    [SerializeField] private List<AnswerOption> answers = new List<AnswerOption>();

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