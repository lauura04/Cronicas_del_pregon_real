using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Diálogo")]
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private bool triggerOnlyOnce = true;

    [Header("Mensaje posterior")]
    [SerializeField] private bool showMessageAfterDialogue;
    [SerializeField]
    private float messageDuration = 3f;
    [SerializeField]
    [TextArea(2, 4)]
    private string messageAfterDialogue;

    [Header("Eventos posteriores")]
    [SerializeField] private UnityEvent onDialogueFinished;

    private bool hasTriggered;

    private void Reset()
    {
        Collider triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        if (dialogue == null)
        {
            Debug.LogError(
                $"No se ha asignado un DialogueData en {gameObject.name}."
            );
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "No se ha encontrado DialogueManager.Instance."
            );
            return;
        }

        hasTriggered = true;

        DialogueManager.Instance.StartDialogue(
            dialogue,
            HandleDialogueFinished
        );
    }

    private void HandleDialogueFinished()
    {
        Debug.Log("Intentando mostrar mensaje");
        if (showMessageAfterDialogue)
        {
            if (MessageUI.Instance == null)
            {
                Debug.LogWarning(
                    "No se ha encontrado MessageUI.Instance."
                );
            }
            else if (string.IsNullOrWhiteSpace(messageAfterDialogue))
            {
                Debug.LogWarning(
                    $"El mensaje posterior está vacío en {gameObject.name}."
                );
            }
            else
            {
                Debug.Log(
    $"showMessageAfterDialogue: {showMessageAfterDialogue} | " +
    $"message: {messageAfterDialogue} | " +
    $"duration: {messageDuration}"
);
                MessageUI.Instance.ShowMessage(
                    messageAfterDialogue, messageDuration
                );
            }
        }

        onDialogueFinished?.Invoke();
    }
}