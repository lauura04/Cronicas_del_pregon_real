using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueStep
{
    public DialogueData dialogue;

    [Header("Event when finished")]
    public UnityEvent onDialogueFinished;

    [SerializeField] private bool advanceAutomatically = true;

    public bool AdvanceAutomatically => advanceAutomatically;
}