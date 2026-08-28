using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
  public static SaveManager Instance {get;private set;}

  private const string CHAPTER_KEY = "UnlockedCHapter";
  public int UnlockedChapter{get;private set;}

  private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
    }

    private void LoadProgress()
    {
        UnlockedChapter = PlayerPrefs.GetInt(CHAPTER_KEY,0);
    }

    public void UnlockChapter(int chapter)
    {
        if (chapter <= UnlockedChapter)
        {
            return;
        }

        UnlockedChapter = chapter;

        PlayerPrefs.SetInt(CHAPTER_KEY, UnlockedChapter);
        PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        UnlockedChapter = 0;
        PlayerPrefs.SetInt(CHAPTER_KEY,0);
        PlayerPrefs.Save();
    }
}
