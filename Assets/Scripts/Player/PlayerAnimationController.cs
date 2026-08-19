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
            if (currentAnimator == frontAnimator)
            {
                currentAnimator.SetBool("IsMoving",true);
            }
        }

        else
        {
            SetDirection(front,frontAnimator);
            frontAnimator.SetBool("IsMoving", false);
        }
    }

   private void UpdateDirection(Vector3 direction)
{
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
    {
        if (direction.x > 0)
        {
            SetDirection(right, rightAnimator);
        }
        else
        {
            SetDirection(left, leftAnimator);
        }
    }
    else
    {
        if (direction.z > 0)
        {
            SetDirection(back, backAnimator);
        }
        else
        {
            SetDirection(front, frontAnimator);

            frontAnimator.SetBool("IsMoving", true);
        }
    }
}

    private void SetDirection(GameObject objectToShow, Animator animatorToUse)
    {
        if(currentDirectionObject == objectToShow)
            return;
        
        front.SetActive(false);
        back.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);

        objectToShow.SetActive(true);

        currentDirectionObject = objectToShow;
        currentAnimator = animatorToUse;
    }
}
