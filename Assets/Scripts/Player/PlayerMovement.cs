using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
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

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (!canMove)
        {
            movementInput = Vector3.zero;
            return;
        }
        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            movementInput = Vector3.zero;
            return;
        }
        MovePlayer();
    }

    private void ReadMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (movementInput.sqrMagnitude > 0.01f)
        {
            lastMovementDirection = movementInput;

            string newDirection = "";

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                newDirection = horizontalInput > 0 ? "Right" : "Left";
            }
            else
            {
                newDirection = verticalInput > 0 ? "Up" : "Down";
            }
            if (newDirection != currentDirection)
            {
                currentDirection = newDirection;
                Debug.Log("Player is moving " + currentDirection);
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 newPosition = playerRigidbody.position + movementInput * moveSpeed * Time.fixedDeltaTime;
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
    }
}
