using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class KeyMinigameManager : MonoBehaviour
{

    public static KeyMinigameManager Instance { get; private set; }
    [Header("UI")]
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite[] characterSprites;

    

    [Header("Settings")]
    [SerializeField] private float requiredTime = 10f;
    [SerializeField] private int maxMistakes = 3;
    [SerializeField] private float timePerKey = 4f;
    [SerializeField] private float timePenalty = 3f;

    [Header("Events")]
    [SerializeField] private UnityEvent onMinigameCompleted;
    [SerializeField] private UnityEvent onMinigameFailed;
    [SerializeField] private DialogueData completeDialogue;
    [SerializeField] private DialogueData failDialouge;

    private float currentTime;
    private float currentKeyTime;
    private float targetTime;
    private int mistakes;

    private int currentSpriteIndex = 0;

    private KeyCode currentKey;

    private bool isPlaying;

    private System.Action onCompletedCallback;
    private System.Action onFailedCallback;

    private readonly KeyCode[] availableKeys =
    {
        KeyCode.B,
        KeyCode.C,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.X,
        KeyCode.Z
    };

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

    private void Start()
    {
        minigamePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        UpdateTimer();
        UpdateKeyTimer();
        CheckInput();
    }

    public void StartMinigame(System.Action onCompleted = null, System.Action onFailed = null)
    {
        onCompletedCallback = onCompleted;
        onFailedCallback = onFailed;

        isPlaying = true;
        currentTime = 0f;
        mistakes = 0;
        targetTime = requiredTime;
        minigamePanel.SetActive(true);
        GenerateNewKey();
        UpdateUI();
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    private void UpdateTimer()
    {
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("0.0") + " / " + targetTime.ToString("0.0");
        if (currentTime >= targetTime)
        {
            CompleteMinigame();
        }
    }

    private void UpdateKeyTimer()
    {
        currentKeyTime += Time.deltaTime;
        if (currentKeyTime >= timePerKey)
        {
            KeyTimeExpired();
        }
    }

    private void CheckInput()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }
        if (Input.GetKeyDown(currentKey))
        {
            CorrectInput();
            return;
        }
        CheckWrongInput();
    }

    private void CheckWrongInput()
    {
        foreach (KeyCode key in availableKeys)
        {
            if (Input.GetKeyDown(key))
            {
                WrongInput();
                return;
            }
        }
    }

    private void CorrectInput()
    {
        ChangeCharacterImage();
        GenerateNewKey();
    }

    private void WrongInput()
    {
        mistakes++;
        UpdateUI();
        if (mistakes >= maxMistakes)
        {
            RestartMinigame();
        }
    }

    private void GenerateNewKey()
    {
        int randomIndex = Random.Range(0, availableKeys.Length);
        currentKey = availableKeys[randomIndex];

        keyText.text = currentKey.ToString();
        currentKeyTime = 0f;
    }

    private void UpdateUI()
    {
        mistakesText.text = "Fallos: " + mistakes + " / " + maxMistakes;
    }

    private void RestartMinigame()
    {
        isPlaying = false;
        onMinigameFailed?.Invoke();
        if (failDialouge != null && DialogueManager.Instance != null)
        {
            minigamePanel.SetActive(false);
            DialogueManager.Instance.StartDialogue(failDialouge, RestartAfterFailDialogue);
        }
        else
        {
            RestartAfterFailDialogue();
        }

    }
    private void RestartAfterFailDialogue()
    {
        currentTime=0f;
        currentKeyTime=0f;
        mistakes=0;
        targetTime=requiredTime;
        minigamePanel.SetActive(true);
        GenerateNewKey();
        UpdateUI();

        isPlaying=true;
    }

    private void CompleteMinigame()
    {
        isPlaying = false;
        minigamePanel.SetActive(false);

        if(completeDialogue!=null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(completeDialogue, FinishMinigame);
        }

        else
        {
            FinishMinigame();
        }
    }

    private void FinishMinigame()
    {
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }

        onMinigameCompleted?.Invoke();
        onCompletedCallback?.Invoke();

        onCompletedCallback = null;
        onFailedCallback = null;
    }

    public void CancelMinigame()
    {
        isPlaying = false;
        minigamePanel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }

    private void KeyTimeExpired()
    {
        mistakes++;
        targetTime += timePenalty;
        UpdateUI();
        if (mistakes >= maxMistakes)
        {
            RestartMinigame();
            return;
        }
        GenerateNewKey();
    }
//funcion para hacer cambiar el sprite de la actuación --> cambiar prox a animator
    private void ChangeCharacterImage()
    {
        if(characterImage==null || characterSprites.Length==0)
            return;
        currentSpriteIndex++;
        if(currentSpriteIndex>=characterSprites.Length)
            currentSpriteIndex=0;
        
        characterImage.sprite = characterSprites[currentSpriteIndex];
    }
}

