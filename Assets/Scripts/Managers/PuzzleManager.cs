using UnityEngine;
using UnityEngine.SceneManagement;
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("Puzzle")]
    [SerializeField] private int totalPieces;

    [Header("UI")]
    [SerializeField] private GameObject puzzlePanel;

    [Header("Dialogue")]
    [SerializeField] private DialogueData puzzleCompletedDialogue;
    [SerializeField] private ItemData Miel;

    private int placedPieces;
    private bool puzzleCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        puzzlePanel.SetActive(false);
    }

    public void OpenPuzzle()
    {
        if (puzzleCompleted)
        {
            Debug.Log("Puzzle ya completado");
            return;
        }

        placedPieces = 0;
        puzzlePanel.SetActive(true);
        HUDManager.Instance.HideHUD();
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }

    }
    public void PiecePlaced()
    {
        placedPieces++;

        if (placedPieces >= totalPieces)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleCompleted = true;
        Debug.Log("Puzzle completado");
        InventoryManager.Instance.AddItem(Miel);

        if (puzzleCompletedDialogue != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(puzzleCompletedDialogue, ClosePuzzle);
        }
        else
        {
            ClosePuzzle();
        }
        
    }


    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
        HUDManager.Instance.ShowHUD();
        if(PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }
}