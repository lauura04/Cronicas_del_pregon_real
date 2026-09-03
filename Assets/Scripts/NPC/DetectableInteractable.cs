using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableInteractable : MonoBehaviour
{
   [Header("Detection")]
   [SerializeField] private GameObject highlightObject;
   [SerializeField] private GameObject keyPrompt;

   [Header("Interaction")]
   [SerializeField] private KeyCode interactionKey = KeyCode.E;

   public KeyCode InteractionKey => interactionKey;

   private void Start()
    {
        SetDetected(false);
    }

    public void SetDetected(bool detected)
    {
        if(highlightObject != null)
        {
            highlightObject.SetActive(detected);
        }
        if(keyPrompt != null)
        {
            keyPrompt.SetActive(detected);
        }
    }
}
