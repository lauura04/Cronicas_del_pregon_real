using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slots;
    [SerializeField] private NecklaceCrafting necklaceCrafting;

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<InventorySlotUI>(true);
        }

        Debug.Log(
            "InventoryUI ha encontrado " +
            slots.Length +
            " slots."
        );
    }

    private void OnEnable()
    {
        Debug.Log("INVENTORY UI ACTIVADA");

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += Refresh;
        }
        else
        {
            Debug.LogError(
                "InventoryManager.Instance es NULL"
            );

            return;
        }

        NecklaceCrafting necklaceCrafting =
            FindFirstObjectByType<NecklaceCrafting>();

        if (necklaceCrafting != null)
        {
            Debug.Log("NecklaceCrafting encontrado desde InventoryUI");

            necklaceCrafting.CraftNecklaceIfReady();
        }
        else
        {
            Debug.Log("No se ha encontrado NecklaceCrafting");
        }

        Refresh();
    }
    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
        }
    }

    public void OpenInventory()
    {
        NecklaceCrafting necklaceCrafting =
            FindFirstObjectByType<NecklaceCrafting>();

        if (necklaceCrafting != null)
        {
            necklaceCrafting.CraftNecklaceIfReady();
        }

       PauseMenuManager.Instance.ShowInventory();
    }

    public void Refresh()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError(
                "No existe InventoryManager al hacer Refresh"
            );

            return;
        }

        var items = InventoryManager.Instance.Items;

        Debug.Log(
            "REFRESH INVENTARIO | Items: " +
            items.Count +
            " | Slots: " +
            slots.Length
        );

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                Debug.Log(
                    "Poniendo " +
                    items[i].itemName +
                    " en slot " +
                    i
                );

                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}