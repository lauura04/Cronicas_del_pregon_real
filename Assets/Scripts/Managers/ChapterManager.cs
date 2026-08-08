using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public enum GameChapter{
        Tutorial,
        Chapter1,
        Chapter2,
        Chapter3
    }
   public static ChapterManager Instance {get;private set;}

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

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();
        }

        Debug.Log($"Capítulo iniciado {newChapter}");
    }
}
   
