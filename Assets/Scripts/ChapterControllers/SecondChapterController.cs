using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChapterController : MonoBehaviour
{
    public static SecondChapterController Instance {get; private set;}

    [Header("Chapter")]
    [SerializeField] private ChapterData secondChapter;

    [Header("Phases")]
    [SerializeField] private ChapterPhaseData introPhase;
    [SerializeField] private ChapterPhaseData investigatePhase;
    [SerializeField] private ChapterPhaseData endPhase;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData secondDialogue;

    [Header("POSITIONS")]
    [SerializeField] private Transform playerDestination;




    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameProgressManager.Instance.StartChapter(secondChapter, introPhase);
        StartCoroutine(InitialSequence());
    }

    private IEnumerator InitialSequence()
    {
        PlayerMovement.Instance.SetMovementEnabled(false);
        bool dialogueFinished = false;
        DialogueManager.Instance.StartDialogue(initialDialogue, ()=>dialogueFinished=true);

        yield return StartCoroutine(PlayerMovement.Instance.AutoMoveTo(playerDestination,1f));

        yield return new WaitUntil(()=>dialogueFinished);

        bool secondDialogueFinished = false;

        DialogueManager.Instance.StartDialogue(secondDialogue, () => secondDialogueFinished = true);

        yield return new WaitUntil(()=>secondDialogueFinished);

        PlayerMovement.Instance.SetMovementEnabled(true);
        GameProgressManager.Instance.SetPhase(investigatePhase);
    }
}
