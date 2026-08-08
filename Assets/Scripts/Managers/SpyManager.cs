
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyManager : MonoBehaviour
{
    public static SpyManager Instance {get; private set;}

    [Header("Interfaz")]
    [SerializeField] private GameObject spyPanel;
    [SerializeField] private TMP_Text spyText;
    [SerializeField] private TMP_Text characterNameText;
    
    private SpyConversation currentConversation;
    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        {
            return;
        }

        currentConversation = conversation;
        spyPanel.SetActive(true);

        UpdateSpyUI();
    }

    public void StopListening()
    {
        currentConversation = null;
        ClearUI();
        spyPanel.SetActive(false);
    }

    private void UpdateSpyUI()
    {
        if (currentConversation == null)
        {
            return;
        }

        DialogueLine line = currentConversation.CurrentLine;
        if (line == null)
        {
            return;
        }

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

}