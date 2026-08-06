using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Start()
    {
        ShowMainMenu();
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
        SceneLoader.Instance.LoadScene("ChapterIntro");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
