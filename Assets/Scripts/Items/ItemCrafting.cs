using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemCrafting : MonoBehaviour
{
    [Header("Required Item IDs")]
    [SerializeField] private string[] requiredItemIds;

    [Header("Result")]
    [SerializeField] private ItemData resultItem;

    [Header("Messages")]
    [SerializeField] private string readyMessage = "Tienes todos los objetos necesarios";
    [SerializeField] private string craftedMessage = "Has construido el objeto";
    [SerializeField] private DialogueData craftedDialogue;

    [Header("Events")]
    [SerializeField] private UnityEvent onCrafted;
    private bool itemReady;
    private bool itemCrafted;

    private void Start()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("ItemCrafting: InventoryManager null");
            return;
        }
        InventoryManager.Instance.OnInventoryChanged += CheckIngredients;
        CheckIngredients();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= CheckIngredients;
        }
    }

    private void CheckIngredients()
    {
        if (itemReady || itemCrafted)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            return;
        }

        foreach (string itemId in requiredItemIds)
        {
            int amount = InventoryManager.Instance.GetItemCountById(itemId);
            if (amount < 1)
                return;

        }
        itemReady = true;

        CraftIfReady();
    }

    public void CraftIfReady()
    {
        if (!itemReady || itemCrafted)
            return;
        if (InventoryManager.Instance == null)
            return;
        itemCrafted=true;
        foreach (string itemId in requiredItemIds)
        {
            InventoryManager.Instance.RemoveItemsById(itemId, 1);
        }
        if (resultItem != null)
        {
            InventoryManager.Instance.AddItem(resultItem);
        }

        
        itemReady=false;
        
        if(craftedDialogue!=null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(craftedDialogue);
        }
        else
        {
            MessageUI.Instance?.ShowMessage(craftedMessage,3f);
        }
        onCrafted?.Invoke();
    }


}
