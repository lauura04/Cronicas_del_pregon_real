using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private GameObject hudPanel;

    [Header("Buttons")]
    [SerializeField] private GameObject inventoryButton;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameplayScene =
            scene.name != "MainMenu" &&
            scene.name != "ChapterIntro";

        SetHUDVisible(isGameplayScene);

        
    }

    public void HideInventoryButton()
    {
        inventoryButton.SetActive(false);
    }

    public void ShowHUD()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
        {
            return;
        }

        SetHUDVisible(true);
    }

    public void HideHUD()
    {
        SetHUDVisible(false);
    }

    private void SetHUDVisible(bool visible)
    {
        if (hudPanel == null)
            return;

        hudPanel.SetActive(visible);
    }
}