using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private IInteractable currentInteractable;

    private void Update()
    {
        FindClosestInteractable();

        if (Input.GetKeyDown(interactionKey) &&
            currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void FindClosestInteractable()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(
            transform.position,
            interactionRadius,
            interactableLayer
        );

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider nearbyCollider in nearbyColliders)
        {
            IInteractable interactable =
                nearbyCollider.GetComponentInParent<IInteractable>();

            if (interactable == null)
            {
                continue;
            }

            float distance = Vector3.SqrMagnitude(
                nearbyCollider.transform.position - transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        currentInteractable = closestInteractable;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            interactionRadius
        );
    }
}