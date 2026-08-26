using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirstChapterController : MonoBehaviour
{
    public static FirstChapterController Instance { get; private set; }

    [Header("Chapter")]
    [SerializeField] private ChapterData firstChapter;
    [Header("Initial Phase")]
    [SerializeField] private ChapterPhaseData introPhase;
    [Header("Second Phase")]
    [SerializeField] private ChapterPhaseData performancePhase;
    [Header("Third Phase")]
    [SerializeField] private ChapterPhaseData investigatePhase;
    [Header("Forth Phase")]
    [SerializeField] private ChapterPhaseData endPhase;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData secondDialogue;
    [SerializeField] private DialogueData thirdDialogue;
    [SerializeField] private DialogueData performanceDialogue;
    [SerializeField] private DialogueData investigationDialogue;

    [Header("CHARACTERS")]
    [SerializeField] private NPCMovement dottore;
    [SerializeField] private Transform dottoreDestination;

    [SerializeField] private NPCMovement arlequino;
    [SerializeField] private Transform arlequinoDestination;

    [SerializeField] private NPCMovement colombina;
    [SerializeField] private Transform colombinaDestination;

    [Header("Posiciones pre Performance")]
    [SerializeField] private Transform dottoreAct;
    [SerializeField] private Transform arlequinoAct;
    [SerializeField] private Transform colombinaAct;
    [SerializeField] private PlayerSpawnPoint performanceSpawnPoint;

    [Header("Posiciones post Performance")]
    [SerializeField] private Transform dottorePost;
    [SerializeField] private Transform arlequinoPost;
    [SerializeField] private Transform colombinaPost;
    [SerializeField] private PlayerSpawnPoint letterSpawnPoint;

    [Header("Letters")]
    [SerializeField] private LetterPuzzleData firstLetterData;
    

    //variables de las que depende la continuidad de las fases
    public bool firstLetter { get; private set; } //si ha acertado o no la primera carta --> enlazar con onCorrect del puzzle

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
        GameProgressManager.Instance.StartChapter(firstChapter, introPhase);
        DialogueManager.Instance.StartDialogue(initialDialogue, StartChapter);

    }

    private void StartChapter()
    {
        Debug.Log("Capítulo 1 iniciado");
        StartCoroutine(StartChapterCoroutine());

    }

    private IEnumerator StartChapterCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        DialogueManager.Instance.StartDialogue(secondDialogue, OnSecondDialogueFinished);
    }

    private void OnSecondDialogueFinished()
    {

        dottore.MoveTo(dottoreDestination, OnNPCArrived);

    }

    private void OnNPCArrived()
    {
        DialogueManager.Instance.StartDialogue(thirdDialogue, OnThirdDialogueFinished);
        colombina.SetMoveSpeed(1.5f);
        arlequino.SetMoveSpeed(1.5f);
        colombina.MoveTo(colombinaDestination, null);
        arlequino.MoveTo(arlequinoDestination, null);

    }
    private void OnThirdDialogueFinished()
    {
        StartCoroutine(ChangeScenePosition());
    }

    private IEnumerator ChangeScenePosition()
    {
        yield return ScreenFade.Instance.FadeOutCoroutine();

        MoveCharactersToPerformance();
        performanceSpawnPoint.TeleportPlayer();

        GameProgressManager.Instance.SetPhase(performancePhase);

        yield return ScreenFade.Instance.FadeInCoroutine();
        StartPerformance();
    }

    private void StartPerformance()
    {
        DialogueManager.Instance.StartDialogue(performanceDialogue, OnPerformanceDialogueFinished);
    }

    private void OnPerformanceDialogueFinished()
    {
        LetterPuzzleManager.Instance.StartPuzzle(firstLetterData);
    }
    private void MoveCharactersToPerformance()
    {
        dottore.transform.SetPositionAndRotation(dottoreAct.position, dottoreAct.rotation);
        colombina.transform.SetPositionAndRotation(colombinaAct.position, colombinaAct.rotation);
        arlequino.transform.SetPositionAndRotation(arlequinoAct.position, arlequinoAct.rotation);
    }

    private void OnPerformanceMinigameCompleted()
    {
        Debug.Log("Actuación completada");
        
        StartCoroutine(ChangeToInvestigation());
    }

    private IEnumerator ChangeToInvestigation()
    {
        yield return ScreenFade.Instance.FadeOutCoroutine();

        MoveCharactersPostPerformance();
        letterSpawnPoint.TeleportPlayer();
        GameProgressManager.Instance.SetPhase(investigatePhase);
        yield return ScreenFade.Instance.FadeInCoroutine();
        StartInvestigation();
    }

    private void MoveCharactersPostPerformance()
    {
        dottore.transform.SetPositionAndRotation(dottorePost.position, dottorePost.rotation);
         colombina.transform.SetPositionAndRotation(colombinaPost.position, colombinaPost.rotation);
        arlequino.transform.SetPositionAndRotation(arlequinoPost.position, arlequinoPost.rotation);
    }

    private void StartInvestigation()
    {
        DialogueManager.Instance.StartDialogue(investigationDialogue);
    }
    
}
