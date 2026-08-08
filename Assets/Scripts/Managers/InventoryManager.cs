using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   public static InventoryManager Instance {get; private set;}

   private List<ItemData> items = new List<ItemData>();

   public IReadOnlyList<ItemData> Items => items;

   private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemData item)
    {
        if(item == null)
        {
            Debug.LogWarning("Se intentó añadir un item vacío");
            return;
        }

        items.Add(item);
        Debug.Log($"Objeto añadido al inventario {item.itemName}");
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }
    
    public void ClearInventory()
    {
        items.Clear();
        Debug.Log("Inventario vaciado");
    }
}
