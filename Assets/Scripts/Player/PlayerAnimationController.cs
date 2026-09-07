using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Directional Graphics")]
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    private Animator frontAnimator;
    private Animator backAnimator;
    private Animator leftAnimator;
    private Animator rightAnimator;

    private GameObject currentDirectionObject;
    private Animator currentAnimator;

    private Vector3 lastDirection = Vector3.down;

    private void Awake()
    {
        frontAnimator = front.GetComponent<Animator>();
        backAnimator = back.GetComponent<Animator>();
        leftAnimator = left.GetComponent<Animator>();
        rightAnimator = right.GetComponent<Animator>();

        SetDirection(front, frontAnimator);
    }

    private void Update()
    {
        Vector3 direction = playerMovement.MovementDirection;

        if (playerMovement.IsMoving)
        {
            UpdateDirection(direction);

            if (currentAnimator != null)
            {
                currentAnimator.SetBool("IsMoving", true);
            }
        }
        else
        {
            if (currentAnimator != null)
            {
                currentAnimator.SetBool("IsMoving", false);
            }

            SetDirection(front, frontAnimator);
        }
    }

    private void UpdateDirection(Vector3 direction)
    {
        Vector3 visualDirection = direction;

        int steps = playerMovement.VisualRotationSteps;

        for (int i = 0; i < steps; i++)
        {
            visualDirection = new Vector3(
                visualDirection.z,
                0f,
                -visualDirection.x
            );
        }

        if (Mathf.Abs(visualDirection.x) >
            Mathf.Abs(visualDirection.z))
        {
            // LEFT / RIGHT
            if (playerMovement.SwapRightLeftSprites)
            {
                if (visualDirection.x > 0)
                {
                    SetDirection(left, leftAnimator);
                }
                else
                {
                    SetDirection(right, rightAnimator);
                }
            }
            else
            {
                if (visualDirection.x > 0)
                {
                    SetDirection(right, rightAnimator);
                }
                else
                {
                    SetDirection(left, leftAnimator);
                }
            }
        }
        else
        {
            // FRONT / BACK
            if (playerMovement.SwapFrontBackSprites)
            {
                if (visualDirection.z > 0)
                {
                    SetDirection(front, frontAnimator);
                }
                else
                {
                    SetDirection(back, backAnimator);
                }
            }
            else
            {
                if (visualDirection.z > 0)
                {
                    SetDirection(back, backAnimator);
                }
                else
                {
                    SetDirection(front, frontAnimator);
                }
            }
        }
    }
    private void SetDirection(
        GameObject objectToShow,
        Animator animatorToUse)
    {
        if (currentDirectionObject == objectToShow)
            return;

        if (currentAnimator != null)
        {
            currentAnimator.SetBool("IsMoving", false);
        }

        front.SetActive(false);
        back.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);

        objectToShow.SetActive(true);

        currentDirectionObject = objectToShow;
        currentAnimator = animatorToUse;
    }
}
