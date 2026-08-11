using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    public ChapterData CurrentChapter { get; private set; }
    public ChapterPhaseData CurrentPhase { get; private set; }
    public static event System.Action<ChapterPhaseData> OnPhaseChanged;

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

    public void StartChapter(ChapterData chapter, ChapterPhaseData initialPhase)
    {
        if (chapter == null)
        {
            Debug.LogError("No asignado chapter");
            return;
        }

        if (initialPhase == null)
        {
            Debug.LogError("No asignada initial phase");
        }

        CurrentChapter = chapter;
        CurrentPhase = initialPhase;
        InventoryManager.Instance?.ClearInventory();
    }
    public void SetPhase(ChapterPhaseData newPhase)
    {
        if (CurrentChapter == null)
        {
            Debug.LogError("No hay ningún capítulo activo.");
            return;
        }

        if (newPhase == null)
        {
            Debug.LogError("La nueva fase es null.");
            return;
        }

        if (!CurrentChapter.ContainsPhase(newPhase))
        {
            Debug.LogError(
                $"{newPhase.name} no pertenece al capítulo actual."
            );
            return;
        }

        CurrentPhase = newPhase;

        Debug.Log(
            $"Nueva fase: {CurrentPhase.name}"
        );

        OnPhaseChanged?.Invoke(CurrentPhase);
    }

}
