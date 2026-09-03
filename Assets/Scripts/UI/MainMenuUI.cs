using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        ShowMainMenu();
        MusicManager.Instance.PlayMusic(menuMusic);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void Play()
    {
        ChapterManager.Instance.StartChapter(GameChapter.Tutorial);
    }

    public void NewGame()
    {
        SaveManager.Instance.ResetProgress();
        ChapterManager.Instance.StartChapter(GameChapter.Tutorial);
    }

    public void ContinueGame()
    {
        int chapter = SaveManager.Instance.UnlockedChapter;

        switch (chapter)
        {
            case 0:
                ChapterManager.Instance.StartChapter(GameChapter.Tutorial);
                break;
            case 1:
                ChapterManager.Instance.StartChapter(GameChapter.Chapter1);
                break;
            case 2:
                ChapterManager.Instance.StartChapter(GameChapter.Chapter2);
                break;
            case 3:
                ChapterManager.Instance.StartChapter(GameChapter.Chapter3);
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
