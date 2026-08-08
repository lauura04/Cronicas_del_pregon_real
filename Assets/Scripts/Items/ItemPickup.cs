using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData item;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if(InventoryManager.Instance == null)
        {
            Debug.LogError("InventarioManager.Instance no encontrada");
            return;
        }

        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }
}
