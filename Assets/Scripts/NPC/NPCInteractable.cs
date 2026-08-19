using UnityEngine;
using UnityEngine.Events;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction / Speak with E")]
    [SerializeField] private PhaseDialogue[] interactionDialogues;
    [SerializeField] private bool requiresInfantaDialogue = false;

    private int currentDialogueIndex;
    private PhaseDialogue lastInteractionPhaseDialogue;
    private DialogueStep currentDialogueStep;

    [Header("Spy with Q")]
    [SerializeField] private SpyConversation spyConversation;
    [SerializeField] private PhaseDialogue[] spyDialogues;
    [SerializeField] private float detectionTime = 5f;
    [SerializeField] private PhaseDialogue[] detectedDialogues;

    [Header("Charm with R")]
    [SerializeField] private bool canBeCharmed;
    [SerializeField] private DialogueData charmDialogue;
    [SerializeField] private bool charmOnlyOnce = true;
    [SerializeField] private bool isSheep;

    [Header("Events")]
    [SerializeField] private UnityEvent onInteractionFinished;
    [SerializeField] private UnityEvent onCharmFinished;

    private bool hasBeenCharmed;
    private bool isBusy;

    private PhaseDialogue currentPhaseDialogue;

    public SpyConversation SpyConversation => spyConversation;

    public float DetectionTime => detectionTime;
    [SerializeField] private CharmableNPC charmableNPC;

    public DialogueData DetectedDialogue =>
        GetDialogueForCurrentPhase(detectedDialogues);


    private void OnEnable()
    {
        GameProgressManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        GameProgressManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void Start()
    {
        UpdateSpyConversation();
    }


    // =========================
    // E - INTERACTION / TALK
    // =========================

    public void Interact()
    {
        if (isBusy)
        {
            return;
        }

        PhaseDialogue phaseDialogue =
            GetPhaseDialogueForCurrentPhase(
                interactionDialogues
            );

        if (phaseDialogue == null ||
            phaseDialogue.dialogues == null ||
            phaseDialogue.dialogues.Length == 0)
        {
            return;
        }

        ChapterPhaseData currentPhase =
            GameProgressManager.Instance.CurrentPhase;

        // Si hemos cambiado de fase, empezamos
        // desde el primer diálogo.
        if (lastInteractionPhaseDialogue != phaseDialogue)
        {
            lastInteractionPhaseDialogue = phaseDialogue;
            currentDialogueIndex = 0;
        }

        // Evitamos superar el último diálogo.
        currentDialogueIndex = Mathf.Clamp(
            currentDialogueIndex,
            0,
            phaseDialogue.dialogues.Length - 1
        );

        currentDialogueStep =
            phaseDialogue.dialogues[currentDialogueIndex];

        if (currentDialogueStep == null ||
            currentDialogueStep.dialogue == null)
        {
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "DialogueManager instance is not found."
            );
            return;
        }

        if(requiresInfantaDialogue && !TutorialController.Instance.HasTalkedToInfanta)
        {
            return;
        }

        isBusy = true;

        DialogueManager.Instance.StartDialogue(
            currentDialogueStep.dialogue,
            HandleInteractionFinished
        );
    }


    // =========================
    // Q - SPY
    // =========================

    public DialogueData GetSpyDialogueForCurrentPhase()
    {
        return GetDialogueForCurrentPhase(
            spyDialogues
        );
    }

    private void UpdateSpyConversation()
    {
        if (spyConversation == null)
        {
            return;
        }

        DialogueData dialogue =
            GetDialogueForCurrentPhase(
                spyDialogues
            );

        spyConversation.SetDialogue(
            dialogue
        );
    }

    private void HandlePhaseChanged(
        ChapterPhaseData newPhase)
    {
        UpdateSpyConversation();
    }


    // =========================
    // R - CHARM
    // =========================

   public void Charm()
{
    if (isBusy ||
        !canBeCharmed ||
        (charmOnlyOnce && hasBeenCharmed))
    {
        return;
    }

    if (isSheep &&
        !TutorialController.Instance.SheepUnlocked)
    {
        Debug.Log(
            "Las ovejas no están desbloqueadas"
        );
        return;
    }

    CharmableNPC charmableNPC =
        GetComponent<CharmableNPC>();

    if (charmableNPC == null)
    {
        Debug.LogError(
            $"{gameObject.name} no tiene CharmableNPC."
        );
        return;
    }

    Debug.Log(
        $"Iniciando minijuego Charm con {gameObject.name}"
    );

    charmableNPC.TryCharm();
}


    // =========================
    // PHASE DIALOGUE SEARCH
    // =========================

    private PhaseDialogue GetPhaseDialogueForCurrentPhase(
    PhaseDialogue[] dialogues)
    {
        if (GameProgressManager.Instance == null)
        {
            Debug.LogError(
                "GameProgressManager instance is not found."
            );
            return null;
        }

        if (dialogues == null ||
            dialogues.Length == 0)
        {
            return null;
        }

        ChapterPhaseData currentPhase =
            GameProgressManager.Instance.CurrentPhase;

        if (currentPhase == null)
        {
            Debug.LogWarning(
                "There is no current phase assigned."
            );
            return null;
        }

        int currentPhaseIndex =
            GameProgressManager.Instance.GetPhaseIndex(
                currentPhase
            );

        if (currentPhaseIndex == -1)
        {
            Debug.LogWarning(
                $"La fase {currentPhase.name} no pertenece al capítulo actual."
            );
            return null;
        }

        PhaseDialogue bestDialogue = null;
        int bestPhaseIndex = -1;

        foreach (PhaseDialogue phaseDialogue in dialogues)
        {
            if (phaseDialogue == null ||
                phaseDialogue.phase == null)
            {
                continue;
            }

            int phaseIndex =
                GameProgressManager.Instance.GetPhaseIndex(
                    phaseDialogue.phase
                );

            // Ignoramos fases que no pertenecen
            // al capítulo actual.
            if (phaseIndex == -1)
            {
                continue;
            }

            // Buscamos el diálogo más cercano
            // perteneciente a la fase actual
            // o a una fase anterior.
            if (phaseIndex <= currentPhaseIndex &&
                phaseIndex > bestPhaseIndex)
            {
                bestDialogue = phaseDialogue;
                bestPhaseIndex = phaseIndex;
            }
        }

        return bestDialogue;
    }

    private DialogueData GetDialogueForCurrentPhase(
      PhaseDialogue[] dialogues)
    {
        PhaseDialogue phaseDialogue =
            GetPhaseDialogueForCurrentPhase(dialogues);

        if (phaseDialogue == null)
        {
            return null;
        }

        if (phaseDialogue.dialogues == null ||
            phaseDialogue.dialogues.Length == 0)
        {
            return null;
        }

        DialogueStep firstStep =
            phaseDialogue.dialogues[0];

        if (firstStep == null)
        {
            return null;
        }

        return firstStep.dialogue;
    }


    // =========================
    // FINISHED EVENTS
    // =========================

    private void HandleInteractionFinished()
    {
        isBusy = false;

        currentDialogueStep?
            .onDialogueFinished?
            .Invoke();

        PhaseDialogue phaseDialogue =
            GetPhaseDialogueForCurrentPhase(
                interactionDialogues
            );

        if (phaseDialogue != null &&
    phaseDialogue.dialogues != null &&
    currentDialogueStep != null)
        {
            if (currentDialogueStep.AdvanceAutomatically &&
                currentDialogueIndex <
                phaseDialogue.dialogues.Length - 1)
            {
                currentDialogueIndex++;
            }
        }

        currentDialogueStep = null;

        onInteractionFinished?.Invoke();
    }


    public void AdvanceInteractionDialogue()
    {
        PhaseDialogue phaseDialogue = GetPhaseDialogueForCurrentPhase(interactionDialogues);

        if (phaseDialogue == null || phaseDialogue.dialogues == null)
        {
            return;
        }

        if (currentDialogueIndex < phaseDialogue.dialogues.Length - 1)
        {
            currentDialogueIndex++;
        }
    }
}