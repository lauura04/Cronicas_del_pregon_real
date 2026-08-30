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

    public void SetMovementMapping(bool invertHorizontal, bool invertVertical, bool swapAxes)
    {
        this.invertHorizontal = invertHorizontal;
        this.invertVertical = invertVertical;
        this.swapAxes = swapAxes;
    }

    public void ResetMovementMapping()
    {
        invertHorizontal=false;
        invertVertical=false;
        swapAxes=false;
    }
    private void Update()
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

        if (invertHorizontal)
        {
            horizontalInput*=-1;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");

        if (invertVertical)
        {
            verticalInput*=-1;
        }

        if (swapAxes)
        {
            float temp = horizontalInput;
            horizontalInput = verticalInput;
            verticalInput = temp;
        }

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
        if (movementInput.sqrMagnitude < 0.01f)
        {
            return;
        }
        Vector3 newPosition = playerRigidbody.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        Vector3 rayOrigin = new Vector3(newPosition.x, playerRigidbody.position.y + groundRayHeight,newPosition.z);

        if(Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayDistance, groundLayer)){
            float feetDistance = playerRigidbody.position.y-capsuleCollider.bounds.min.y;

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
    }

//para hacer "animación automática" de movimiento
    
    public IEnumerator AutoMoveTo(Transform destination, float speed = 3f)
    {
        canMove = false;
        isAutoMoving = true;
        
        while (Vector3.Distance(playerRigidbody.position, destination.position) > 0.05f)
        {
            Vector3 direction = destination.position-playerRigidbody.position;
            direction.y=0f;
            direction.Normalize();

            movementInput = direction;

            Vector3 newPosition = playerRigidbody.position + direction*speed*Time.fixedDeltaTime;
            Vector3 rayOrigin = new Vector3(newPosition.x, playerRigidbody.position.y + groundRayHeight, newPosition.z);

            if(Physics.Raycast(rayOrigin,Vector3.down, out RaycastHit hit, groundRayDistance, groundLayer))
            {
                float feetDistance = playerRigidbody.position.y - capsuleCollider.bounds.min.y;

                newPosition.y = hit.point.y + feetDistance + groundSkin;
            }

            playerRigidbody.MovePosition(newPosition);

            yield return new WaitForFixedUpdate();
        }
        movementInput = Vector3.zero;
        isAutoMoving = false;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }
    
}
