using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance {get;private set;}
    [SerializeField] private AudioClip tutorialClip;

    [Header("Chapter")]
    [SerializeField] private ChapterData tutorialChapter;

    [Header("Initial Phase")]
    [SerializeField] private ChapterPhaseData introPhase;
    [Header("Second Phase")]
    [SerializeField] private ChapterPhaseData giftIdea;
    [Header("Third Phase")]
    [SerializeField] private ChapterPhaseData findMaterials;
    [Header("Forth Phase")]
    [SerializeField] private ChapterPhaseData end;   


    [SerializeField] private DialogueData initialDialogue;
    
    //variables de las que dependen las subfases del FindMaterials 
    public bool MonksUnlocked {get; private set;}
    public bool SheepUnlocked{get; private set;}

    public bool HasTalkedToInfanta{get;private set;}

    private void Awake()
    {
        if(Instance!=null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        GameProgressManager.Instance.StartChapter(tutorialChapter, introPhase);
        MusicManager.Instance.PlayMusic(tutorialClip);
        DialogueManager.Instance.StartDialogue(
            initialDialogue, StartTutorial
        );
    }

    public void SetPhase(ChapterPhaseData newPhase)
    {
        GameProgressManager.Instance.SetPhase(newPhase);
        Debug.Log($"Tutorial phase changed to {newPhase}");
    }

    public void UnlockMonks()
    {
        if (MonksUnlocked)
        {
            return;
        }

        MonksUnlocked = true;
        Debug.Log("Monjes desbloqueados");
        CheckMaterialsProgress();
    }

    public void UnlockSheeps()
    {
        if (SheepUnlocked)
        {
            return;
        }
        SheepUnlocked = true;
        Debug.Log("Ovejas desbloqueadas");
        CheckMaterialsProgress();
    }

    private void CheckMaterialsProgress(){ //me la podria cargar
        if (MonksUnlocked && SheepUnlocked)
        {
            Debug.Log("Todo desbloqueado");
        }

    }
    public void StartTutorial()
    {
        // Aquí puedes agregar la lógica para iniciar el tutorial
        Debug.Log("Tutorial iniciado.");
    }

    //funcion para construir el collar
    
    public bool CanCollectWool()
    {
        return SheepUnlocked && GameProgressManager.Instance.CurrentPhase == findMaterials;
    }

    public void InfantaDialogueFinished()
    {
        HasTalkedToInfanta = true;
    }

    public void EndTutorial()
    {
        ChapterManager.Instance.StartChapter(GameChapter.Chapter1);
    }

}
