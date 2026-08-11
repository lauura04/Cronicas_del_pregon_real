
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
   [SerializeField] private Image itemIcon;
   private ItemData currentItem;
   public ItemData CurrentItem => currentItem;

   public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item == null)
        {
            ClearSlot();
            return;
        }
        itemIcon.sprite = item.Icon;
        itemIcon.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite= null;
        itemIcon.enabled = false;
    }
}
