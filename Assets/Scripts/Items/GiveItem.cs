using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
   [SerializeField] private ItemData item;
   [SerializeField] private bool giveOnlyOnce = true;

   [Header("Phase Requirement")]
   [SerializeField] private ChapterPhaseData requiredPhase;

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

       
        if (GameProgressManager.Instance == null)
        {
            Debug.LogError("No hay GameProgressManagerInstance");
            return;
        }

        if(requiredPhase!=null && GameProgressManager.Instance.CurrentPhase != requiredPhase)
        {
            Debug.Log($"{gameObject.name} no puede entregar {item.itemName} en la fase actual");
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
