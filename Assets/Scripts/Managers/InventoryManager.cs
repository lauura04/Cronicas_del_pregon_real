using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<ItemData> items = new List<ItemData>();

    public IReadOnlyList<ItemData> Items => items;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Se intentó añadir un item vacío");
            return false;
        }

        items.Add(item);
        OnInventoryChanged?.Invoke();
        Debug.Log(
         "OBJETO AÑADIDO: " + item.itemName +
         " | Objetos en inventario: " + items.Count
     );
        return true;
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(ItemData item)
    {
        if (item == null)
        {
            return;
        }

        if (items.Remove(item))
        {
            OnInventoryChanged?.Invoke();
        }
    }

    public void ClearInventory()
    {
        items.Clear();
        OnInventoryChanged?.Invoke();
        Debug.Log("Inventario vaciado");
    }

    public int GetItemCountById(string itemId)
    {
        int count = 0;

        foreach (ItemData item in items)
        {
            if (item != null && item.ItemId == itemId)
            {
                count++;
            }
        }

        return count;
    }

    public bool RemoveItemsById(string itemId, int amount)
    {
        if (GetItemCountById(itemId) < amount)
        {
            return false;
        }

        int removed = 0;

        for (int i = items.Count - 1; i >= 0 && removed < amount; i--)
        {
            if (items[i] != null && items[i].ItemId == itemId)
            {
                items.RemoveAt(i);
                removed++;
            }
        }

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool HasItemById(string itemId)
    {
        return GetItemCountById(itemId)>0;
    }
}
