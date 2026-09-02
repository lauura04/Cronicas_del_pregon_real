using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdChapterController : MonoBehaviour
{
    public static ThirdChapterController Instance { get; private set; }

    [Header("Chapter")]
    [SerializeField] private ChapterData thirdChapter;

    //fases en saber
    [Header("Phases")]
    [SerializeField] private ChapterPhaseData introPhase;

    [Header("DIALOGUES")]
    [SerializeField] private DialogueData initialDialogue;

     private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameProgressManager.Instance.StartChapter(thirdChapter, introPhase);
        DialogueManager.Instance.StartDialogue(initialDialogue, () => { }); // entrada de las pavas 
    }

}
