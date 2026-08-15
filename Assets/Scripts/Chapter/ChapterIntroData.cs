using UnityEngine;
[CreateAssetMenu(fileName = "NewChapterIntro", menuName = "Game/Chapter Intro")]
public class ChapterIntroData : ScriptableObject
{
    [SerializeField] private GameChapter chapter;
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private string gameplayScene;

    //meter después
    [SerializeField] private string chapterTitle;
    [SerializeField] private string chapterSubtitle; //o fecha
    [SerializeField] private Sprite background; 
    [SerializeField] private AudioClip music;

    public GameChapter Chapter => chapter;
    public DialogueData Dialogue => dialogue;
    public string GameplayScene => gameplayScene;   
}
