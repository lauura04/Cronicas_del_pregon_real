using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance {get;private set;}

    public enum TutorialPhase
    {
        Intro,
        GiftIdea,
        FindMaterials,
        End
    }

    public TutorialPhase CurrentPhase {get; private set;}
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
