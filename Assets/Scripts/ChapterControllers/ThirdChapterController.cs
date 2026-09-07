using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdChapterController : MonoBehaviour
{
    public static ThirdChapterController Instance { get; private set; }

    [Header("Chapter")]
    [SerializeField] private ChapterData thirdChapter;
    [SerializeField] private AudioClip music;

    [Header("Phases")]
    [SerializeField] private ChapterPhaseData introPhase;
    [SerializeField] private ChapterPhaseData getThingsPhase;
    [SerializeField] private ChapterPhaseData lookForMusicianPhase;
    [SerializeField] private ChapterPhaseData endPhase;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData secondDialogue;
    [SerializeField] private DialogueData thirdDialogue;

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

    private bool itemsObjectiveCompleted = false;

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
        GameProgressManager.Instance.StartChapter(
            thirdChapter,
            introPhase
        );

        StartCoroutine(StartChapterFade());

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic(music);
        }

        /*
         * Nos suscribimos aquí y no en OnEnable,
         * porque así nos aseguramos de que el
         * InventoryManager ya esté inicializado.
         */
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged +=
                CheckCandlesAndHoney;

            // Por si ya tenemos miel o velas al empezar.
            CheckCandlesAndHoney();
        }
        else
        {
            Debug.LogError(
                "ThirdChapterController: InventoryManager.Instance es NULL"
            );
        }
    }

    private IEnumerator StartChapterFade()
    {
        yield return null;

        if (ScreenFade.Instance != null)
        {
            yield return ScreenFade.Instance.FadeInCoroutine();
        }

        StartCoroutine(InitialSequence());
    }

    private IEnumerator InitialSequence()
    {
        herald.MoveTo(
            heraldFirstDestination,
            null
        );

        bool dialogueFinished = false;

        DialogueManager.Instance.StartDialogue(
            initialDialogue,
            () => dialogueFinished = true
        );

        yield return new WaitUntil(
            () => dialogueFinished
        );

        yield return new WaitForSeconds(0.5f);

        herald.MoveTo(
            heraldSecondDestination,
            () => heraldObject.SetActive(false)
        );

        yield return new WaitForSeconds(1f);

        firstWoman.MoveTo(
            firstWomanDestination,
            null
        );

        secondWoman.MoveTo(
            secondWomanDestination,
            null
        );

        thirdWoman.MoveTo(
            thirdWomanDestination,
            null
        );

        bool secondDialogueFinished = false;

        DialogueManager.Instance.StartDialogue(
            secondDialogue,
            () => secondDialogueFinished = true
        );

        yield return new WaitUntil(
            () => secondDialogueFinished
        );

        yield return new WaitForSeconds(1f);

        OnNPCsArrived();
    }

    private void OnNPCsArrived()
    {
        DialogueManager.Instance.StartDialogue(
            thirdDialogue,
            NextPhase
        );
    }

    private void NextPhase()
    {
        GameProgressManager.Instance.SetPhase(
            getThingsPhase
        );

        firstWoman.MoveTo(
            firstWomanDestination2,
            () => firstWoman.gameObject.SetActive(false)
        );

        secondWoman.MoveTo(
            secondWomanDestination2,
            () => secondWoman.gameObject.SetActive(false)
        );

        thirdWoman.MoveTo(
            thirdWomanDestination2,
            () => thirdWoman.gameObject.SetActive(false)
        );
    }

    public void EntryTower()
    {
        StartCoroutine(
            TeleportWithFade(towerSpawnPoint1)
        );
    }

    public void OutTower()
    {
        StartCoroutine(
            TeleportWithFade(towerSpawnPoint2)
        );
    }

    private IEnumerator TeleportWithFade(
        PlayerSpawnPoint spawnPoint
    )
    {
        if (ScreenFade.Instance != null)
        {
            yield return StartCoroutine(
                ScreenFade.Instance.FadeOutCoroutine()
            );
        }

        spawnPoint.TeleportPlayer();

        yield return null;

        if (ScreenFade.Instance != null)
        {
            yield return StartCoroutine(
                ScreenFade.Instance.FadeInCoroutine()
            );
        }
    }

    public void OpenPuzzle()
    {
        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.OpenPuzzle();
        }
    }

    public void UnlockedIsabel()
    {
        unlockIsabel = true;

        if (isabelObject != null)
        {
            isabelObject.SetActive(true);
        }
    }

    public void MoveIsabelToDestination()
    {
        if (
            isabel != null &&
            isabelDestination != null
        )
        {
            isabel.MoveTo(
                isabelDestination,
                () => isabel.gameObject.SetActive(false)
            );
        }
        else
        {
            Debug.LogWarning(
                "Isabel o isabelDestination no están asignados."
            );
        }
    }

    public void CheckCandlesAndHoney()
    {
        /*
         * Una vez completado no queremos volver
         * a ejecutar esta lógica cada vez que
         * cambie el inventario.
         */
        if (itemsObjectiveCompleted)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning(
                "InventoryManager no está asignado."
            );

            return;
        }

        bool hasCandles =
            InventoryManager.Instance.HasItemById(
                candlesID
            );

        bool hasHoney =
            InventoryManager.Instance.HasItemById(
                honeyID
            );

        Debug.Log(
            "Comprobando objetos -> " +
            "Velas: " + hasCandles +
            " | Miel: " + hasHoney
        );

        if (hasCandles && hasHoney)
        {
            itemsObjectiveCompleted = true;

            Debug.Log(
                "El jugador tiene velas y miel. " +
                "Avanzando a endPhase."
            );

            GameProgressManager.Instance.SetPhase(
                endPhase
            );

            if (heraldObject != null)
            {
                heraldObject.SetActive(true);
            }

            if (
                herald != null &&
                heraldFirstDestination != null
            )
            {
                herald.transform.position =
                    heraldFirstDestination.position;
            }
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -=
                CheckCandlesAndHoney;
        }
    }

    public void End()
    {
        if (ScreenFade.Instance != null)
        {
            ScreenFade.Instance.FadeOutAndIn();
        }

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("End");
        }
    }
}