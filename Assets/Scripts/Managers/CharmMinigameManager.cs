using UnityEngine;

public class CharmMinigameManager : MonoBehaviour
{
    public static CharmMinigameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private CharmMinigameUI minigameUI;

    [Header("Circle Settings")]
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float minScale = 0.5f;

    [Header("Speed")]
    [SerializeField] private float scaleSpeed = 0.8f;

    [Header("Success Zone")]
    [SerializeField] private float successMinScale = 0.8f;
    [SerializeField] private float successMaxScale = 1.1f;

    private CharmableNPC currentNPC;

    private float currentScale;
    private bool shrinking;

    private bool isActive;


    private bool canCheckInput;

    public bool IsActive => isActive;

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

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        UpdateCircle();


        if (!canCheckInput)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                canCheckInput = true;
            }

            return;
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckAttempt();
        }
    }

    public void StartMinigame(CharmableNPC npc)
    {
        Debug.Log(
            $"START MINIGAME con {npc?.gameObject.name}"
        );

        if (isActive)
        {
            Debug.Log("Minijuego ya activo.");
            return;
        }

        if (npc == null)
        {
            Debug.LogError("NPC NULL");
            return;
        }

        currentNPC = npc;
        currentScale = maxScale;
        shrinking = true;
        isActive = true;
        canCheckInput = false;

        PlayerMovement.Instance?.SetMovementEnabled(false);

        if (minigameUI != null)
        {
            Debug.Log("SHOW UI CHARM");

            minigameUI.Show();
            minigameUI.SetCircleScale(currentScale);
            minigameUI.SetSuccessState(false);
        }
        else
        {
            Debug.LogError("MINIGAME UI ES NULL");
        }
    }

    private void UpdateCircle()
    {
        if (shrinking)
        {
            currentScale -=
                scaleSpeed * Time.deltaTime;

            if (currentScale <= minScale)
            {
                currentScale = minScale;

                shrinking = false;
            }
        }
        else
        {
            currentScale +=
                scaleSpeed * Time.deltaTime;

            if (currentScale >= maxScale)
            {
                currentScale = maxScale;

                shrinking = true;
            }
        }

        if (minigameUI != null)
        {
            minigameUI.SetCircleScale(
                currentScale
            );

            bool isSuccess =
                currentScale >= successMinScale &&
                currentScale <= successMaxScale;

            minigameUI.SetSuccessState(
                isSuccess
            );
        }
    }

    private void CheckAttempt()
    {
        bool success =
            currentScale >= successMinScale &&
            currentScale <= successMaxScale;

        if (success)
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

   private void Success()
{
    Debug.Log("CHARM SUCCESS");

    isActive = false;

    if (minigameUI != null)
    {
        minigameUI.Hide();
    }

    PlayerMovement.Instance?.SetMovementEnabled(true);

    currentNPC?.CharmSucceeded();

    currentNPC = null;
}
   private void Fail()
{
    isActive = false;

    if (minigameUI != null)
    {
        minigameUI.Hide();
    }

    CharmableNPC failedNPC = currentNPC;
    currentNPC = null;

    if (failedNPC != null)
    {
        failedNPC.CharmFailed();
    }
    else
    {
        PlayerMovement.Instance?.SetMovementEnabled(true);
    }
}
}