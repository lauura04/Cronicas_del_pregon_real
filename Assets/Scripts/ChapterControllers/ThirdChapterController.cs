using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdChapterController : MonoBehaviour
{
    public static ThirdChapterController Instance { get; private set; }

    [Header("Chapter")]
    [SerializeField] private ChapterData thirdChapter;

    //fases en saber
    [Header("Phases")]
    [SerializeField] private ChapterPhaseData introPhase;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData secondDialogue;
    [SerializeField] private DialogueData thirdDialogue; // mujeres con bufon

    [Header("POSITIONS")]
    [SerializeField] private NPCMovement herald;
    [SerializeField] private Transform heraldFirstDestination;
    [SerializeField] private Transform heraldSecondDestination;
    [SerializeField] private NPCMovement firstWoman;
    [SerializeField] private Transform firstWomanDestination;
    [SerializeField] private NPCMovement secondWoman;
    [SerializeField] private Transform secondWomanDestination;
    [SerializeField] private NPCMovement thirdWoman;
    [SerializeField] private Transform thirdWomanDestination;

     private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameProgressManager.Instance.StartChapter(thirdChapter, introPhase);
        StartCoroutine(InitialSequence());
    }

    private IEnumerator InitialSequence()
    {
        herald.MoveTo(heraldFirstDestination, null);
        bool dialogueFinished = false;
        DialogueManager.Instance.StartDialogue(initialDialogue, () => dialogueFinished = true); // se va el heraldo y entrada de las pavas 
        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(0.5f);
        herald.MoveTo(heraldSecondDestination, null);
        yield return new WaitForSeconds(1f);
        firstWoman.MoveTo(firstWomanDestination,null);
        secondWoman.MoveTo(secondWomanDestination, null);
        thirdWoman.MoveTo(thirdWomanDestination, null);
        bool secondDialogueFinished = false;
        DialogueManager.Instance.StartDialogue(secondDialogue, ()=> secondDialogueFinished = true); // se va el heraldo y entrada de las pavas
        yield return new WaitUntil(() => secondDialogueFinished);
        yield return new WaitForSeconds(1f);
        OnNPCsArrived();
        
    }

    private void OnNPCsArrived()
    {
        DialogueManager.Instance.StartDialogue(thirdDialogue, null);
    }

    

}
