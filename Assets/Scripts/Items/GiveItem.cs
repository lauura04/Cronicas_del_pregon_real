using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
   [SerializeField] private ItemData item;
   [SerializeField] private bool giveOnlyOnce = true;

   private bool hasGivenItem;

   public void Give()
    {
        if(giveOnlyOnce && hasGivenItem)
        {
            return;
        }

        if (item == null)
        {
            Debug.LogError($"No hay ningún ItemData asignado a {item}");
            return;
        }

        if(InventoryManager.Instance == null)
        {
            Debug.LogError("No hay InventoryManager.Instance");
            return;
        }

        InventoryManager.Instance.AddItem(item);
        hasGivenItem = true;
        Debug.Log($"{gameObject.name} ha entregado {item.itemName}");
    }
}
