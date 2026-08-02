using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
   public static DialogueManager Instance{get;private set;}

   private List<string> currentDialogue;
   private int currentLineIndex; //para almacenar el indice de la linea de dialogo en la que se encuentra
   
   private TMP_Text dialogueText;
   private System.Action onDialogueFinished;

   private void Awake()
    {
        
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(List<string> dialogue, TMP_Text textComponent, System.Action onFinished)
    {
        currentDialogue = dialogue;
        dialogueText = textComponent;
        onDialogueFinished = onFinished;
        currentLineIndex = 0;

    }
}
