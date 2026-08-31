using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InquisitorPuzzleController : MonoBehaviour
{
    public static InquisitorPuzzleController Instance { get; private set; }

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData firstDialogue;
    [SerializeField] private DialogueData missingEvidenceDialogue;
    [SerializeField] private DialogueData correctEvidenceDialogue;

    [Header("ITEM IDS")]
    [SerializeField] private string coinsID = "bolsa";
    [SerializeField] private string catID = "gata";
    [SerializeField] private string sheetID = "sabana_manchada";




    public bool ItemsUnlocked { get; private set; }

    private bool firstConversationDone;
    private bool puzzleCompleted;

    private void Awake()
    {
        Instance = this;
    }

    public void Interact()
    {
        if (!firstConversationDone)
        {
            DialogueManager.Instance.StartDialogue(firstDialogue, HandleFirstDialogueFinished);
            return;
        }
        if (puzzleCompleted)
        {
            return;
        }
        CheckEvidence();

    }

    private void HandleFirstDialogueFinished()
    {
        firstConversationDone = true;
        ItemsUnlocked = true;
    }


    public void CheckEvidence()
    {
        bool hasCat = InventoryManager.Instance.HasItemById(catID);
        bool hasCoin = InventoryManager.Instance.HasItemById(coinsID);
        bool hasSheet = InventoryManager.Instance.HasItemById(sheetID);

        if (hasCat && hasCoin && hasSheet)
        {
            puzzleCompleted = true;
            DialogueManager.Instance.StartDialogue(correctEvidenceDialogue, OnDialogueFinished);

        }
        else
        {
            DialogueManager.Instance.StartDialogue(missingEvidenceDialogue, null);
        }
    }

    private void OnDialogueFinished()
    {
        SecondChapterController.Instance
    .CompleteInquisitorPuzzle();
    }
}
