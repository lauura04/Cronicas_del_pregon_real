using UnityEngine;
using UnityEngine.Events;

public class CharmableNPC : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData failedCharmDialogue;
    [SerializeField] private DialogueData sucessCharmDialogue;

    [Header("Events")]
    [SerializeField] private UnityEvent onCharmSuccess;

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
        if (isCharmed)
        {
            return;
        }

        isCharmed = true;
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
            return;
        }

        DialogueManager.Instance?.StartDialogue(
            failedCharmDialogue,
            null
        );
    }
}