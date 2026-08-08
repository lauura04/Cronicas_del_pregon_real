using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    public enum GameChapter
    {
        Tutorial,
        Chapter1,
        Chapter2,
        Chapter3
    }

    public int CurrentPhase { get; private set; }

    public GameChapter CurrentChapter { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartChapter(GameChapter chapter)
    {
        CurrentChapter = chapter;
        CurrentPhase = 0;
    }
    public void SetPhase(int phase)
    {
        CurrentPhase = phase;
    }

    public void NextPhase()
    {
        CurrentPhase++;
    }
}
