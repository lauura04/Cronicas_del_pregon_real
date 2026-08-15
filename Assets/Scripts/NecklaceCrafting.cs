using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NecklaceCrafting : MonoBehaviour
{
    [Header("Ingredient IDs")]
    [SerializeField] private string pearlsId = "perlas";
    [SerializeField] private string claspId = "enganche";
    [SerializeField] private string threadId = "hilo";

    [Header("Result")]
    [SerializeField] private ItemData necklaceItem;

    [Header("Events")]
    [SerializeField] private UnityEvent onNecklaceCrafted;

    private bool necklaceReady;
    private bool necklaceCrafted;

    private void Start()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError(
                "NecklaceCrafting: InventoryManager.Instance es NULL"
            );
            return;
        }

        Debug.Log("NecklaceCrafting conectado al inventario");

        InventoryManager.Instance.OnInventoryChanged += CheckIngredients;

        // Por si ya teníamos algún ingrediente
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
    Debug.Log("CHECK INGREDIENTS EJECUTADO");

    if (necklaceReady || necklaceCrafted)
    {
        return;
    }

    if (InventoryManager.Instance == null)
    {
        Debug.LogError("InventoryManager es NULL");
        return;
    }

    int pearls =
        InventoryManager.Instance.GetItemCountById(pearlsId);

    int clasp =
        InventoryManager.Instance.GetItemCountById(claspId);

    int thread =
        InventoryManager.Instance.GetItemCountById(threadId);

    Debug.Log(
        $"Ingredientes collar → " +
        $"Perlas: {pearls} | " +
        $"Enganche: {clasp} | " +
        $"Hilo: {thread}"
    );

    if (pearls < 1 || clasp < 1 || thread < 1)
    {
        return;
    }

    necklaceReady = true;

    Debug.Log("COLLAR LISTO");

    MessageUI.Instance?.ShowMessage(
        "Has construido el collar, abre el inventario",
        3f
    );
}

    private void ShowCraftedMessage()
    {

        if (MessageUI.Instance == null)
        {
            Debug.LogError("MessageUI.Instance no existe.");
            return;
        }

        MessageUI.Instance.ShowMessage(
            "Has construido el collar",
            3f
        );
    }

   public void CraftNecklaceIfReady()
{
    Debug.Log("CraftNecklaceIfReady ejecutado");

    if (!necklaceReady)
    {
        Debug.Log("El collar todavía no está listo");
        return;
    }

    if (necklaceCrafted)
    {
        Debug.Log("El collar ya fue construido");
        return;
    }

    if (InventoryManager.Instance == null)
    {
        Debug.LogError("InventoryManager.Instance es NULL");
        return;
    }

    Debug.Log("Quitando ingredientes del collar...");

    InventoryManager.Instance.RemoveItemsById(
        pearlsId,
        1
    );

    InventoryManager.Instance.RemoveItemsById(
        claspId,
        1
    );

    InventoryManager.Instance.RemoveItemsById(
        threadId,
        1
    );

    Debug.Log("Añadiendo collar...");

    InventoryManager.Instance.AddItem(
        necklaceItem
    );

    necklaceCrafted = true;
    necklaceReady = false;

    Debug.Log("COLLAR CONSTRUIDO EN INVENTARIO");

    onNecklaceCrafted?.Invoke();
}

}
