using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    
   public static ChapterManager Instance {get;private set;}
   [SerializeField] private ChapterIntroData[] chapterIntros;
   public GameChapter CurrentChapter {get; private set;}

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

    public void StartChapter(GameChapter newChapter)
    {
        CurrentChapter = newChapter;

       SceneLoader.Instance.LoadScene("ChapterIntro");

        Debug.Log($"Capítulo iniciado {newChapter}");
    }

    public ChapterIntroData GetCurrentIntro()
    {
        foreach(ChapterIntroData intro in chapterIntros)
        {
            if(intro.Chapter == CurrentChapter)
            {
                return intro;
            }

            
        }
        Debug.LogError("No existe introducción para " + CurrentChapter);
            return null;
    }


}
   
