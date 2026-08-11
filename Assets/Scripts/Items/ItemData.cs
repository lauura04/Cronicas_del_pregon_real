using UnityEngine;
[CreateAssetMenu(fileName="New Item", menuName="Inventory/Item")]

public class ItemData : ScriptableObject
{
   [Header("Información")]
   public string itemName;

   [TextArea(2,4)]
   public string description;

   [Header("Visual")]
   public Sprite Icon;

   [SerializeField] private string itemId;
   public string ItemId => itemId;
}
