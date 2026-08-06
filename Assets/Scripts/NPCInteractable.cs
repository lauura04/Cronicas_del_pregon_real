using UnityEngine;
using UnityEngine.Events;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction with E")]
    [SerializeField] private DialogueData interactionDialogue;

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
        if (isBusy)
        {
            return;
        }

        if (interactionDialogue == null)
        {
            Debug.LogError($"Interaction dialogue is not assigned in {gameObject.name}");
            return;
        }
        if(DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance is not found in the scene.");
            return;
        }

        isBusy = true;
        DialogueManager.Instance.StartDialogue(interactionDialogue, HandleInteractionFinished);
    }

    public void Charm()
    {
        if(isBusy || !canBeCharmed || (charmOnlyOnce && hasBeenCharmed))
        {
            return;
        }

        if(charmDialogue == null)
        {
            Debug.LogError($"Charm dialogue is not assigned in {gameObject.name}");
            return;
        }

         if(DialogueManager.Instance == null)
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