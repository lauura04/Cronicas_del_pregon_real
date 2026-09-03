using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionSensor : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Input")]
    [SerializeField] private KeyCode detectionKey = KeyCode.F;

    private readonly List<DetectableInteractable> detectedObjects = new List<DetectableInteractable>();
    private bool detecting;

    private void Update()
    {
        if (Input.GetKeyDown(detectionKey))
        {
            StartDetection();
        }

        if(Input.GetKeyUp(detectionKey))
        {
            StopDetection();
        }
        if (detecting)
        {
            UpdateDetection();
        }
    }

    private void StartDetection()
    {
        detecting = true;
        UpdateDetection();
    }

    private void StopDetection()
    {
        detecting = false;
        foreach (DetectableInteractable interactable in detectedObjects)
        {
            if (interactable != null)
            {
                interactable.SetDetected(false);
            }
        }

        detectedObjects.Clear();
    }

    private void UpdateDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            interactableLayer
        );

        HashSet<DetectableInteractable> currentObjects =
            new HashSet<DetectableInteractable>();

        foreach (Collider col in colliders)
        {
            DetectableInteractable detectable =
                col.GetComponentInParent<DetectableInteractable>();

            if (detectable == null)
                continue;

            currentObjects.Add(detectable);

            if (!detectedObjects.Contains(detectable))
            {
                detectedObjects.Add(detectable);
                detectable.SetDetected(true);
            }
        }

        for (int i = detectedObjects.Count - 1; i >= 0; i--)
        {
            DetectableInteractable detectable = detectedObjects[i];

            if (detectable == null ||
                !currentObjects.Contains(detectable))
            {
                if (detectable != null)
                    detectable.SetDetected(false);

                detectedObjects.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
