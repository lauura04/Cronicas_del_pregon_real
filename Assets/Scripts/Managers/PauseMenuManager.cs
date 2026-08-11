using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject inventoryPanel;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public void PauseGame()
    {
        if (IsPaused)
            return;

        IsPaused = true;

        HUDManager.Instance?.HideHUD();

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!IsPaused)
            return;

        IsPaused = false;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        HUDManager.Instance?.ShowHUD();
    }

    public void TogglePause()
    {
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void GoToMainMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);

        HUDManager.Instance?.HideHUD();

        SceneLoader.Instance.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Salir");
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void ShowInventory()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
    public void BackToPauseMenu()
    {
        inventoryPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}