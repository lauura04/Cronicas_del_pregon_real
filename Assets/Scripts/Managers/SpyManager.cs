
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpyManager : MonoBehaviour
{
    public static SpyManager Instance { get; private set; }

    [Header("Interfaz")]
    [SerializeField] private GameObject spyPanel;
    [SerializeField] private TMP_Text spyText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Slider spyProgressBar;

    private SpyConversation currentConversation;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (spyPanel != null)
        {
            spyPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentConversation == null)
        {
            return;
        }

        UpdateSpyUI();
    }

    public void StartListening(SpyConversation conversation)
{
    if (conversation == null)
        return;

    Debug.Log(
        $"START LISTENING -> {conversation.gameObject.name}",
        conversation.gameObject
    );

    currentConversation = conversation;

    spyPanel.SetActive(true);

    if (spyProgressBar != null)
        spyProgressBar.value = 0f;

    UpdateSpyUI();
}

public void StopListening()
{
    Debug.Log("STOP LISTENING");

    currentConversation = null;

    if (spyProgressBar != null)
        spyProgressBar.value = 0f;

    ClearUI();
    spyPanel.SetActive(false);
}

    private void UpdateSpyUI()
{
    if (currentConversation == null)
    {
        Debug.Log("UPDATE SPY -> NO CONVERSATION");
        return;
    }

    DialogueLine line = currentConversation.CurrentLine;

    if (line == null)
    {
        Debug.Log(
            $"UPDATE SPY -> {currentConversation.gameObject.name} | LINE NULL"
        );
        return;
    }

    Debug.Log(
        $"UPDATE SPY -> {currentConversation.gameObject.name}" +
        $" | TEXT: '{currentConversation.CurrentText}'"
    );

    spyText.text = currentConversation.CurrentText;

    if (line.Character != null)
    {
        characterNameText.text = line.Character.CharacterName;
    }
    else
    {
        characterNameText.text = "";
    }
}

    private void ClearUI()
    {
        spyText.text = "";
        characterNameText.text = "";
    }

    public void UpdateSpyProgress(
    float currentTime,
    float maxTime)
    {
        if (spyProgressBar == null)
        {
            return;
        }

        if (maxTime <= 0f)
        {
            spyProgressBar.value = 0f;
            return;
        }

        spyProgressBar.value =
            Mathf.Clamp01(currentTime / maxTime);
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
        currentConversation = null;

        if (spyProgressBar != null)
            spyProgressBar.value = 0f;

        ClearUI();

        if (spyPanel != null)
            spyPanel.SetActive(false);

        Debug.Log("SpyManager reset en escena: " + scene.name);
    }

}