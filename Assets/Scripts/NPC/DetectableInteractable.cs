using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DetectableInteractable : MonoBehaviour
{
   [Header("Detection")]
   [SerializeField] private GameObject interactionPrompt;
   [SerializeField] private TMP_Text interactionText;

   [Header("Interaction")]
   [SerializeField] private KeyCode interactionKey = KeyCode.E;

   public KeyCode InteractionKey => interactionKey;

   private void Start()
    {
        if(interactionText != null)
        {
            interactionText.text = interactionKey.ToString();
        }

        SetDetected(false);
    }

    public void SetDetected(bool detected)
    {
        if(interactionPrompt != null)
        {
            interactionPrompt.SetActive(detected);
        }
        
    }
}
