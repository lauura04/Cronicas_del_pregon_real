
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
   [SerializeField] private Image itemIcon;
   [SerializeField] private TMP_Text itemName;
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

        itemName.text = item.itemName;
        itemName.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite= null;
        itemIcon.enabled = false;

        itemName.text="";
        itemName.gameObject.SetActive(false);
    }
}
