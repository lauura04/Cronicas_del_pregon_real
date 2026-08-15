using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] slots;

    public bool IsOpen =>
        inventoryPanel != null &&
        inventoryPanel.activeSelf;

    private void Awake()
    {
        Instance = this;

        if (slots == null || slots.Length == 0)
        {
            slots =
                GetComponentsInChildren<InventorySlotUI>(true);
        }

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void ToggleInventory()
    {
        if (IsOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        if (inventoryPanel == null)
        {
            return;
        }

        NecklaceCrafting necklaceCrafting =
            FindFirstObjectByType<NecklaceCrafting>();

        if (necklaceCrafting != null)
        {
            necklaceCrafting.CraftNecklaceIfReady();
        }

        inventoryPanel.SetActive(true);

        Refresh();
    }

    public void CloseInventory()
    {
        if (inventoryPanel == null)
        {
            return;
        }

        inventoryPanel.SetActive(false);
    }

    public void Refresh()
    {
        if (InventoryManager.Instance == null)
        {
            return;
        }

        var items =
            InventoryManager.Instance.Items;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}