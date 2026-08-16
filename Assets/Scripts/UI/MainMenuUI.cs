using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
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

    public void Play()
    {
       ChapterManager.Instance.StartChapter(GameChapter.Tutorial);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
