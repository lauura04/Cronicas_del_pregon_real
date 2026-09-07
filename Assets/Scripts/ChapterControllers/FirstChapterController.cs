using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirstChapterController : MonoBehaviour
{
    public static FirstChapterController Instance { get; private set; }
    [SerializeField] private AudioClip firstChapterClip;
    [SerializeField] private AudioClip performanceClip;
    [SerializeField] private AudioClip queenSound;
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
    [SerializeField] private DialogueData lastDialogue;

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
    [SerializeField] private LetterPuzzleData firstLetterPuzzle;
    [SerializeField] private LetterPuzzleData secondLetterPuzzle;
    [SerializeField] private DialogueData noLetterDialogue;

    [SerializeField] private DialogueData correctFirstLetterDialogue;

    [SerializeField] private GameObject secondLetterObject;



    [Header("People")]
    [SerializeField] private GameObject performancePeople;
    [SerializeField] private GameObject investigationPeople;
    [SerializeField] private GameObject normalPeople;



    //variables de las que depende la continuidad de las fases
    public bool firstLetter { get; private set; } //si ha acertado o no la primera carta --> enlazar con onCorrect del puzzle
    public bool secondLetter { get; private set; }
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
        firstLetter = false;
        if (secondLetterObject != null)
        {
            secondLetterObject.SetActive(false);
        }
         MusicManager.Instance.PlayMusic(firstChapterClip);
        GameProgressManager.Instance.StartChapter(firstChapter, introPhase);

        StartCoroutine(StartChapterFade());
        
    }
     private IEnumerator StartChapterFade()
    {
        yield return null;
        if(ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeInCoroutine();
        }
        DialogueManager.Instance.StartDialogue(
            initialDialogue, StartChapter
        );
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
        performancePeople.SetActive(true);
        normalPeople.SetActive(false);

        yield return ScreenFade.Instance.FadeInCoroutine();
        StartPerformance();
    }

    private void StartPerformance()
    {
        MusicManager.Instance.StopMusic();
        MusicManager.Instance.PlayMusic(performanceClip);
        DialogueManager.Instance.StartDialogue(performanceDialogue, OnPerformanceDialogueFinished);
    }

    private void OnPerformanceDialogueFinished()
    {
        KeyMinigameManager.Instance.StartMinigame(OnPerformanceMinigameCompleted);
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
        MusicManager.Instance.StopMusic();
        MusicManager.Instance.PlayMusic(firstChapterClip);

        StartCoroutine(ChangeToInvestigation());
    }

    private IEnumerator ChangeToInvestigation()
    {
        yield return ScreenFade.Instance.FadeOutCoroutine();

        MoveCharactersPostPerformance();
        letterSpawnPoint.TeleportPlayer();
        GameProgressManager.Instance.SetPhase(investigatePhase);
        performancePeople.SetActive(false);
        investigationPeople.SetActive(true);
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
    public void CheckLetterProgress()
    {
        if (!firstLetter)
        {
            CheckFirstLetter();
            return;
        }
        if (!secondLetter)
        {
            CheckSecondLetter();
            return;
        }
    }
    public void CheckFirstLetter()
    {
        if (InventoryManager.Instance.HasItemById("Carta1"))
        {
            LetterPuzzleManager.Instance.StartPuzzle(firstLetterPuzzle, OnFirstLetterCorrect);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(noLetterDialogue, null);
        }
    }

    public void OnFirstLetterCorrect()
    {
        Debug.Log("Primera carta descifrada correctamente");
        firstLetter = true;
        DialogueManager.Instance.StartDialogue(correctFirstLetterDialogue, UnlockSecondLetter);
    }

    private void UnlockSecondLetter()
    {
        if (secondLetterObject != null)
            secondLetterObject.SetActive(true);
        Debug.Log("Segunda carta desbloqueada");
    }

    public void CheckSecondLetter()
    {
        if (InventoryManager.Instance.HasItemById("Carta2"))
        {
            LetterPuzzleManager.Instance.StartPuzzle(secondLetterPuzzle, onSecondLetterCorrect);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(noLetterDialogue, null);
        }
    }

    public void onSecondLetterCorrect()
    {
        SFXManager.Instance.PlaySFX(queenSound);
        Debug.Log("Segunda carta descifrada correctamente");
        DialogueManager.Instance.StartDialogue(lastDialogue, onLastDialogueFinished);
        secondLetter = true;
    }

    public void onLastDialogueFinished()
    {
        SaveManager.Instance.UnlockChapter(2);
        ChapterManager.Instance.StartChapter(GameChapter.Chapter2);
    }

}
