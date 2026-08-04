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

    public Vector3 MovementDirection => movementInput;
    public bool IsMoving => movementInput.sqrMagnitude > 0.01f;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void ReadMovementInput()
    {
        float horizontalInput= Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        movementInput = new Vector3(horizontalInput,0f,verticalInput).normalized;
    }

    private void MovePlayer()
    {
        Vector3 newPosition = playerRigidbody.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(newPosition);
    }
}
