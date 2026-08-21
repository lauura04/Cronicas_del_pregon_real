using UnityEngine;

public class WoolExchange : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private string woolId = "lana";
    [SerializeField] private int woolRequired = 3;

    [Header("Reward")]
    [SerializeField] private ItemData pearlItem;

    [Header("Dialogues")]
    [SerializeField] private DialogueData successDialogue;
    [SerializeField] private DialogueData notEnoughDialogue;
   

    private bool exchangeCompleted;

    public void TryExchange()
    {
        
        if (exchangeCompleted)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance no existe.");
            return;
        }

        int woolCount =
            InventoryManager.Instance.GetItemCountById(woolId);

        Debug.Log($"Lanas en inventario: {woolCount}");

        if (woolCount < woolRequired)
        {
            ShowDialogue(notEnoughDialogue);
            return;
        }

        bool removed =
            InventoryManager.Instance.RemoveItemsById(
                woolId,
                woolRequired
            );

        if (!removed)
        {
            Debug.LogError("No se han podido eliminar las lanas.");
            return;
        }

        InventoryManager.Instance.AddItem(pearlItem);

        exchangeCompleted = true;

        ShowDialogue(successDialogue);
    }

    private void ShowDialogue(DialogueData dialogue)
    {
        if (dialogue == null)
        {
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance no existe.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogue);
    }
}