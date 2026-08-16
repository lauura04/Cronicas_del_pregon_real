using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeyMinigameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text timerText;

    [Header("Settings")]
    [SerializeField] private float requiredTime = 10f;
    [SerializeField] private int maxMistakes = 3;

    [Header("Events")]
    [SerializeField] private UnityEvent onMinigameCompleted;
    [SerializeField] private UnityEvent onMinigameFailed;

    private float currentTime;
    private int mistakes;

    private KeyCode currentKey;

    private bool isPlaying;

    private readonly KeyCode[] availableKeys =
    {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Z
    };

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
        CheckInput();
    }

    public void StartMinigame()
    {
        isPlaying = true;
        currentTime = 0f;
        mistakes = 0;

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
        timerText.text = currentTime.ToString("0.0") + " / " + requiredTime.ToString("0.0");
        if (currentTime >= requiredTime)
        {
            CompleteMinigame();
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
        GenerateNewKey();
    }

    private void WrongInput()
    {
        mistakes++;
        UpdateUI();
        if (mistakes > maxMistakes)
        {
            RestartMinigame();
        }
    }

    private void GenerateNewKey()
    {
        int randomIndex = Random.Range(0, availableKeys.Length);
        currentKey = availableKeys[randomIndex];

        keyText.text = currentKey.ToString();
    }

    private void UpdateUI()
    {
        mistakesText.text = "Fallos: "+ mistakes + " / " + maxMistakes;
    }

    private void RestartMinigame()
    {
        onMinigameFailed?.Invoke();
        currentTime = 0f;
        mistakes = 0;

        GenerateNewKey();
        UpdateUI();
    }

    private void CompleteMinigame()
    {
        
    }
}

