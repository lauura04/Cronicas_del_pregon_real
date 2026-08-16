using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirstChapterController : MonoBehaviour
{
    public static FirstChapterController Instance {get; private set;}

    [Header("Chapter")]
    [SerializeField] private ChapterData firstChapter;
    [Header("Initial Phase")]
    [SerializeField] private ChapterPhaseData introPhase;
    [Header("Second Phase")]
    [SerializeField] private ChapterPhaseData performancePhase;
    [Header("Third Phase")]
    [SerializeField] private ChapterPhaseData investigatePhase;
    [Header("Forth Phase")]
    [SerializeField] private ChapterPhaseData endPhase;

    [SerializeField] private DialogueData initialDialogue;

    //variables de las que depende la continuidad de las fases
    public bool firstLetter {get; private set;} //si ha acertado o no la primera carta --> enlazar con onCorrect del puzzle

    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        GameProgressManager.Instance.StartChapter(firstChapter, introPhase);
        DialogueManager.Instance.StartDialogue(initialDialogue, StartChapter);
    }

    private void StartChapter()
    {
        Debug.Log("Capítulo 1 iniciado");
    }
    
}
