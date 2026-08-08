using UnityEngine;
using UnityEngine.Events;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction/Speak with E")]
    [SerializeField] private DialogueData interactionDialogue;

    [Header("Spy with Q")]
    [SerializeField] private SpyConversation spyConversation;
    [SerializeField] private float detectionTime = 5f;
    [SerializeField] private DialogueData detectedDialogue;


    public SpyConversation SpyConversation => spyConversation;
    public float DetectionTime => detectionTime;
    public DialogueData DetectedDialogue => detectedDialogue;

    [Header("Charm with R")]
    [SerializeField] private bool canBeCharmed;
    [SerializeField] private DialogueData charmDialogue;
    [SerializeField] private bool charmOnlyOnce = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onInteractionFinished;
    [SerializeField] private UnityEvent onCharmFinished;

    private bool hasBeenCharmed;
    private bool isBusy;

    public void Interact()
    {
        if (isBusy || interactionDialogue == null)
        {
            return;
        }


        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance is not found in the scene.");
            return;
        }

        isBusy = true;
        DialogueManager.Instance.StartDialogue(interactionDialogue, HandleInteractionFinished);
    }

    public void Charm()
    {
        if (isBusy || !canBeCharmed || (charmOnlyOnce && hasBeenCharmed))
        {
            return;
        }

        if (charmDialogue == null) //si quito esto podría encantar sin necesidad de dialogo --> revisar a futuro
        {
            Debug.LogError($"Charm dialogue is not assigned in {gameObject.name}");
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance is not found in the scene.");
            return;
        }

        isBusy = true;
        DialogueManager.Instance.StartDialogue(charmDialogue, HandleCharmFinished);
    }

    private void HandleInteractionFinished()
    {
        isBusy = false;
        onInteractionFinished?.Invoke();
    }

    private void HandleCharmFinished()
    {
        isBusy = false;
        hasBeenCharmed = true;
        onCharmFinished?.Invoke();
    }

    
}