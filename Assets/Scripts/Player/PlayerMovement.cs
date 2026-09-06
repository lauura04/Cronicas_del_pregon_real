using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayHeight = 2f;
    [SerializeField] private float groundRayDistance = 5f;
    [SerializeField] private float groundSkin = 0.02f;

    private CapsuleCollider capsuleCollider;



    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody playerRigidbody;
    private Vector3 movementInput;

    private Vector3 lastMovementDirection = Vector3.forward;
    private string currentDirection = "";

    public Vector3 MovementDirection => movementInput;
    public bool IsMoving => movementInput.sqrMagnitude > 0.01f;

    private bool canMove = true;
    public bool CanMove => canMove;
    private bool isAutoMoving = false;

    //variables cambio de eje x angulo
    private bool invertHorizontal;
    private bool invertVertical;
    private bool swapAxes;
    public bool SwapAxes => swapAxes;

    private bool hasPendingMapping = false;

    private bool pendingInvertHorizontal;
    private bool pendingInvertVertical;
    private bool pendingSwapAxes;
    public static PlayerMovement Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        DontDestroyOnLoad(gameObject);
    }

   public void SetMovementMapping(
    bool invertHorizontal,
    bool invertVertical,
    bool swapAxes)
{
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    float verticalInput = Input.GetAxisRaw("Vertical");

    bool isPressingMovementKey =
        Mathf.Abs(horizontalInput) > 0.01f ||
        Mathf.Abs(verticalInput) > 0.01f;

    if (isPressingMovementKey)
    {
        pendingInvertHorizontal = invertHorizontal;
        pendingInvertVertical = invertVertical;
        pendingSwapAxes = swapAxes;

        hasPendingMapping = true;

        return;
    }

    ApplyMovementMapping(
        invertHorizontal,
        invertVertical,
        swapAxes
    );
}
private void ApplyMovementMapping(
    bool invertHorizontal,
    bool invertVertical,
    bool swapAxes)
{
    this.invertHorizontal = invertHorizontal;
    this.invertVertical = invertVertical;
    this.swapAxes = swapAxes;

    hasPendingMapping = false;
}

    public void ResetMovementMapping()
{
    SetMovementMapping(
        false,
        false,
        false
    );
}    private void Update()
    {
        if (isAutoMoving)
        {
            return;
        }
        if (!canMove)
        {
            movementInput = Vector3.zero;
            return;
        }
        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        if (isAutoMoving)
        {
            return;
        }
        if (!canMove)
        {
            movementInput = Vector3.zero;
            return;
        }
        MovePlayer();
    }

   private void ReadMovementInput()
{
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    float verticalInput = Input.GetAxisRaw("Vertical");

    // Si hay un cambio de controles pendiente,
    // esperamos hasta que el jugador suelte TODO.
    if (hasPendingMapping)
    {
        bool noMovementInput =
            Mathf.Abs(horizontalInput) < 0.01f &&
            Mathf.Abs(verticalInput) < 0.01f;

        if (noMovementInput)
        {
            ApplyMovementMapping(
                pendingInvertHorizontal,
                pendingInvertVertical,
                pendingSwapAxes
            );
        }
    }

    if (invertHorizontal)
    {
        horizontalInput *= -1;
    }

    if (invertVertical)
    {
        verticalInput *= -1;
    }

    if (swapAxes)
    {
        float temp = horizontalInput;
        horizontalInput = verticalInput;
        verticalInput = temp;
    }

    movementInput =
        new Vector3(
            horizontalInput,
            0f,
            verticalInput
        ).normalized;

    if (movementInput.sqrMagnitude > 0.01f)
    {
        lastMovementDirection = movementInput;

        string newDirection = "";

        if (Mathf.Abs(horizontalInput) >
            Mathf.Abs(verticalInput))
        {
            newDirection =
                horizontalInput > 0
                    ? "Right"
                    : "Left";
        }
        else
        {
            newDirection =
                verticalInput > 0
                    ? "Up"
                    : "Down";
        }

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;

            Debug.Log(
                "Player is moving " +
                currentDirection
            );
        }
    }
}

    private void MovePlayer()
    {
        if (movementInput.sqrMagnitude < 0.01f)
        {
            return;
        }
        Vector3 newPosition = playerRigidbody.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        Vector3 rayOrigin = new Vector3(newPosition.x, playerRigidbody.position.y + groundRayHeight, newPosition.z);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayDistance, groundLayer))
        {
            float feetDistance = playerRigidbody.position.y - capsuleCollider.bounds.min.y;

            newPosition.y = hit.point.y + feetDistance + groundSkin;
        }
        playerRigidbody.MovePosition(newPosition);
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        movementInput = Vector3.zero;

        if (!canMove)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }

public void TeleportTo(Vector3 position)
{
    movementInput = Vector3.zero;

    playerRigidbody.velocity = Vector3.zero;
    playerRigidbody.angularVelocity = Vector3.zero;

    playerRigidbody.position = position;

    Physics.SyncTransforms();
}

    //para hacer "animación automática" de movimiento

    public IEnumerator AutoMoveTo(Transform destination, float speed = 3f)
    {
        canMove = false;
        isAutoMoving = true;

        while (Vector3.Distance(
            new Vector3(playerRigidbody.position.x, 0f, playerRigidbody.position.z),
            new Vector3(destination.position.x, 0f, destination.position.z)
        ) > 0.02f)
        {
            Vector3 direction = destination.position - playerRigidbody.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                direction.Normalize();
                movementInput = direction;
                lastMovementDirection = direction;
            }

            Vector3 targetPosition = new Vector3(
                destination.position.x,
                playerRigidbody.position.y,
                destination.position.z
            );

            Vector3 newPosition = Vector3.MoveTowards(
                playerRigidbody.position,
                targetPosition,
                speed * Time.fixedDeltaTime
            );

            Vector3 rayOrigin = new Vector3(
                newPosition.x,
                playerRigidbody.position.y + groundRayHeight,
                newPosition.z
            );

            if (Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                groundRayDistance,
                groundLayer))
            {
                float feetDistance =
                    playerRigidbody.position.y - capsuleCollider.bounds.min.y;

                newPosition.y =
                    hit.point.y + feetDistance + groundSkin;
            }

            playerRigidbody.MovePosition(newPosition);

            yield return new WaitForFixedUpdate();
        }

        // Dejamos al jugador EXACTAMENTE en el destino
        Vector3 finalPosition = playerRigidbody.position;
        finalPosition.x = destination.position.x;
        finalPosition.z = destination.position.z;

        playerRigidbody.MovePosition(finalPosition);

        // Muy importante para que el Animator detecte que ha parado
        movementInput = Vector3.zero;

        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;

        isAutoMoving = false;
    }
}
