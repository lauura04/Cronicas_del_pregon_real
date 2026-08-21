using UnityEngine;
using UnityEngine.Events;

public class CharmableNPC : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData failedCharmDialogue;
    [SerializeField] private DialogueData sucessCharmDialogue;

    [Header("Events")]
    [SerializeField] private UnityEvent onCharmSuccess;

    [SerializeField] private AudioClip charmSound;

    private bool isCharmed;

    public bool IsCharmed => isCharmed;

    public void TryCharm()
    {
        if (isCharmed)
        {
            return;
        }

        if (CharmMinigameManager.Instance == null)
        {
            Debug.LogError(
                "CharmMinigameManager Instance is null."
            );

            return;
        }

        CharmMinigameManager.Instance.StartMinigame(this);
    }

   public void CharmSucceeded()
{
    Debug.Log(
        $"CharmSucceeded llamado en {gameObject.name}"
    );

    if (isCharmed)
    {
        return;
    }

    isCharmed = true;
    SFXManager.Instance.PlaySFX(charmSound);

    DialogueManager.Instance?.StartDialogue(
        sucessCharmDialogue,
        null
    );

    onCharmSuccess?.Invoke();
}

   public void CharmFailed()
{
    if (failedCharmDialogue == null)
    {
        PlayerMovement.Instance?.SetMovementEnabled(true);
        return;
    }

    if (DialogueManager.Instance == null)
    {
        PlayerMovement.Instance?.SetMovementEnabled(true);
        return;
    }

    DialogueManager.Instance.StartDialogue(
        failedCharmDialogue,
        HandleFailedCharmDialogueFinished
    );
}

private void HandleFailedCharmDialogueFinished()
{
    PlayerMovement.Instance?.SetMovementEnabled(true);
}
}