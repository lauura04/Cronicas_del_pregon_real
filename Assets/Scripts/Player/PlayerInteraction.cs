using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Controles")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode charmKey = KeyCode.R;

    
    private IInteractable currentInteractable;

    private void Update()
    {
       

        FindClosestInteractable();

        if (Input.GetKeyDown(interactionKey))
        {
            currentInteractable.Interact();
        }

        if(Input.GetKeyDown(charmKey))
        {
            currentInteractable.Charm();
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

            float distance = (
                nearbyCollider.ClosestPoint(transform.position) -
                transform.position
            ).sqrMagnitude;

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