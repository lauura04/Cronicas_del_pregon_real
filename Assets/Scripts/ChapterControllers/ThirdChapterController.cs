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
    [SerializeField] private ChapterPhaseData getThingsPhase;
    [SerializeField] private ChapterPhaseData lookForMusicianPhase;
    [SerializeField] private ChapterPhaseData endPhase;

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
    [SerializeField] private Transform firstWomanDestination2;
    [SerializeField] private NPCMovement secondWoman;
    [SerializeField] private Transform secondWomanDestination;
    [SerializeField] private Transform secondWomanDestination2;
    [SerializeField] private NPCMovement thirdWoman;
    [SerializeField] private Transform thirdWomanDestination;
    [SerializeField] private Transform thirdWomanDestination2;
    [SerializeField] private NPCMovement isabel;
    [SerializeField] private Transform isabelDestination;
    [SerializeField] private PlayerSpawnPoint towerSpawnPoint1;
    [SerializeField] private PlayerSpawnPoint towerSpawnPoint2;
    [Header("Characters")]
    [SerializeField] private GameObject isabelObject;
    [SerializeField] private GameObject heraldObject;

    [Header("ITEMS")]
    [SerializeField] private string candlesID = "velas";
    [SerializeField] private string honeyID = "miel";

    private bool unlockIsabel = false;
    public bool UnlockIsabel => unlockIsabel;

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
        GameProgressManager.Instance.StartChapter(thirdChapter, getThingsPhase);

        StartCoroutine(InitialSequence());
    }

    private IEnumerator InitialSequence()
    {
        herald.MoveTo(heraldFirstDestination, null);
        bool dialogueFinished = false;
        DialogueManager.Instance.StartDialogue(initialDialogue, () => dialogueFinished = true); // se va el heraldo y entrada de las pavas 
        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(0.5f);
        herald.MoveTo(heraldSecondDestination, () => heraldObject.SetActive(false));
        yield return new WaitForSeconds(1f);
        firstWoman.MoveTo(firstWomanDestination, null);
        secondWoman.MoveTo(secondWomanDestination, null);
        thirdWoman.MoveTo(thirdWomanDestination, null);
        bool secondDialogueFinished = false;
        DialogueManager.Instance.StartDialogue(secondDialogue, () => secondDialogueFinished = true); // se va el heraldo y entrada de las pavas
        yield return new WaitUntil(() => secondDialogueFinished);
        yield return new WaitForSeconds(1f);
        OnNPCsArrived();

    }

    private void OnNPCsArrived()
    {
        DialogueManager.Instance.StartDialogue(thirdDialogue, NextPhase);
    }
    private void NextPhase()
    {
        GameProgressManager.Instance.SetPhase(
            getThingsPhase
        );
        firstWoman.MoveTo(firstWomanDestination2, () => firstWoman.gameObject.SetActive(false));
        secondWoman.MoveTo(secondWomanDestination2, () => secondWoman.gameObject.SetActive(false));
        thirdWoman.MoveTo(thirdWomanDestination2, () => thirdWoman.gameObject.SetActive(false));

    }

    public void EntryTower()
    {

        StartCoroutine(TeleportWithFade(towerSpawnPoint1));
    }

    public void OutTower()
    {
        StartCoroutine(TeleportWithFade(towerSpawnPoint2));
    }

    private IEnumerator TeleportWithFade(PlayerSpawnPoint spawnPoint)
    {
        yield return StartCoroutine(ScreenFade.Instance.FadeOutCoroutine());

        spawnPoint.TeleportPlayer();

        yield return null;
        yield return StartCoroutine(ScreenFade.Instance.FadeInCoroutine());
    }


    public void OpenPuzzle()
    {
        PuzzleManager.Instance.OpenPuzzle();
    }

    public void UnlockedIsabel()
    {
        unlockIsabel = true;
        isabelObject.SetActive(true);
        //aparición de la reina en un point
    }

    public void MoveIsabelToDestination()
    {
        if (isabel != null && isabelDestination != null)
        {
            isabel.MoveTo(isabelDestination, () => isabel.gameObject.SetActive(false));
        }
        else
        {
            Debug.LogWarning("Isabel o isabelDestination no están asignados.");
        }

    }

    public void CheckCandlesAndHoney()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager no está asignado.");
            return;
        }

        bool hasCandles = InventoryManager.Instance.HasItemById(candlesID);
        bool hasHoney = InventoryManager.Instance.HasItemById(honeyID);

        if (hasCandles && hasHoney)
        {
            Debug.Log("El jugador tiene ambos objetos: velas y miel. Avanzando a la fase final.");
            GameProgressManager.Instance.SetPhase(endPhase);
            heraldObject.SetActive(true);
            heraldObject.transform.position = heraldFirstDestination.position;
        }
    }

    private void OnEnable()
    {
        if(InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += CheckCandlesAndHoney;
        }
    }

    private void OnDisable()
    {
        if(InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= CheckCandlesAndHoney;
        }
    }

    public void End()
    {
        
        ScreenFade.Instance.FadeOut();
    }


}
