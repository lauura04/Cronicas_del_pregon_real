using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
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

    private void Awake()
    {
        frontAnimator = front.GetComponent<Animator>();
        backAnimator = back.GetComponent<Animator>();
        leftAnimator = left.GetComponent<Animator>();
        rightAnimator = right.GetComponent<Animator>();

        SetDirection(front, frontAnimator);
    }

    public void UpdateDirection(Vector3 direction)
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
            }
        }

        if (currentAnimator != null)
        {
            currentAnimator.SetBool("IsMoving", true);
        }
    }

    public void StopMoving()
    {
        
        if (currentAnimator != null)
        {
            currentAnimator.SetBool("IsMoving", false);
        }

        
        SetDirection(front, frontAnimator);

        
        frontAnimator.SetBool("IsMoving", false);
    }

    private void SetDirection(
        GameObject objectToShow,
        Animator animatorToUse)
    {
        if (currentDirectionObject == objectToShow)
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