using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    
    [SerializeField] private DialogueData initialDialogue;

    private void Start()
    {
        
        DialogueManager.Instance.StartDialogue(
            initialDialogue, StartTutorial
        );
    }

    public void StartTutorial()
    {
        // Aquí puedes agregar la lógica para iniciar el tutorial
        Debug.Log("Tutorial iniciado.");
    }
}
