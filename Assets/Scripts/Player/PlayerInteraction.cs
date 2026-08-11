using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Controles")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode spyKey = KeyCode.Q;
    [SerializeField] private KeyCode charmKey = KeyCode.R;

    private IInteractable currentInteractable;

    private NPCInteractable spyingNPC;
    private float spyTimer;

    private void Update()
    {
        FindClosestInteractable();

        HandleInteraction();
        HandleSpy();
        HandleCharm();
    }

    private void HandleInteraction()
    {
        if (!Input.GetKeyDown(interactionKey))
        {
            return;
        }

        if (currentInteractable == null)
        {
            return;
        }

        if (PlayerMovement.Instance != null &&
            !PlayerMovement.Instance.CanMove)
        {
            return;
        }

        currentInteractable.Interact();
    }

    private void HandleCharm()
    {
        if (!Input.GetKeyDown(charmKey))
        {
            return;
        }

        if (CharmMinigameManager.Instance != null &&
            CharmMinigameManager.Instance.IsActive)
        {
            return;
        }

        if (currentInteractable == null)
        {
            return;
        }

        CharmableNPC charmableNPC =
            (currentInteractable as MonoBehaviour)?
            .GetComponentInParent<CharmableNPC>();

        if (charmableNPC == null)
        {
            return;
        }

        charmableNPC.TryCharm();
    }

    private void HandleSpy()
    {
        NPCInteractable npc =
            currentInteractable as NPCInteractable;

        bool canSpy =
            Input.GetKey(spyKey) &&
            npc != null &&
            npc.SpyConversation != null;

        if (!canSpy)
        {
            StopSpying();
            return;
        }

        if (spyingNPC != npc)
        {
            StopSpying();

            spyingNPC = npc;
            spyTimer = 0f;

            if (SpyManager.Instance != null)
            {
                SpyManager.Instance.StartListening(
                    npc.SpyConversation
                );
            }
        }

        spyTimer += Time.deltaTime;

        if (spyTimer >= npc.DetectionTime)
        {
            BeDetected();
        }
    }

    private void StopSpying()
    {
        if (spyingNPC == null)
        {
            return;
        }

        if (SpyManager.Instance != null)
        {
            SpyManager.Instance.StopListening();
        }

        spyingNPC = null;
        spyTimer = 0f;
    }

    private void BeDetected()
    {
        NPCInteractable detectedNPC = spyingNPC;

        if (SpyManager.Instance != null)
        {
            SpyManager.Instance.StopListening();
        }

        spyingNPC = null;
        spyTimer = 0f;

        if (detectedNPC != null &&
            detectedNPC.DetectedDialogue != null)
        {
            DialogueManager.Instance?.StartDialogue(
                detectedNPC.DetectedDialogue
            );
        }
    }

    private void FindClosestInteractable()
    {
        Collider[] nearbyColliders =
            Physics.OverlapSphere(
                transform.position,
                interactionRadius,
                interactableLayer
            );

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider nearbyCollider in nearbyColliders)
        {
            IInteractable interactable =
                nearbyCollider
                .GetComponentInParent<IInteractable>();

            if (interactable == null)
            {
                continue;
            }

            float distance =
                (
                    nearbyCollider
                    .ClosestPoint(transform.position)
                    - transform.position
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