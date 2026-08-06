using UnityEngine;
using UnityEngine.Events;

public class SpyDialogueInteractable : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogue;

    [Header("Configuration")]
    [SerializeField] private bool canRepeat = true;

    [Header("Events")]
    [SerializeField] private UnityEvent onDialogueFinished;

    private bool hasBeenPlayed;

    public void Interact()
    {
        if (!canRepeat && hasBeenPlayed)
        {
            return;
        }

        if (dialogue == null)
        {
            Debug.LogError($"Dialogue data is not assigned in {gameObject.name}");
            return;
        }
        if(DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance is not found in the scene.");
            return;
        }

        hasBeenPlayed = true;
        if(MessageUI.Instance != null)
        {
            MessageUI.Instance.HideMessage();
        }

        DialogueManager.Instance.StartDialogue(dialogue, HandleDialogueFinished);
    }

    private void HandleDialogueFinished()
    {
        onDialogueFinished?.Invoke();
    }

    public void Charm()
    {
        // Implement charm behavior if needed
    }
}
