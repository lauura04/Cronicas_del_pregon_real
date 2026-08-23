using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.05f;

    
    [Header("Animation")] // TODAVIA NO ESTÁ ANIMADO
    [SerializeField] private Animator animator;
    private bool isMoving;
    public bool IsMoving => isMoving;

    public void MoveTo(Transform target, System.Action onArrival=null)
    {
        if (target == null)
        {
            Debug.LogWarning("NPC Movement no tiene destino");
            return;
        }
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(target,onArrival));
    }

    private IEnumerator MoveCoroutine(Transform target, System.Action onArrival)
    {
        isMoving = true;

        
        if (animator != null)
        {
            animator.SetBool("IsMoving", true);
        }

        while(GetHorizontalDistance(target.position) > stoppingDistance)
        {
            Vector3 direction = target.position - transform.position;

            //NO MOVIMIENTO VERTICAL
            direction.y = 0f;

            direction.Normalize();
            
            //UpdateAnimation(direction);

            Vector3 nextPosition = transform.position + direction*moveSpeed*Time.deltaTime;

            //conservar altura
            nextPosition.y = transform.position.y;

            transform.position = nextPosition;

            yield return null;
        }

        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.rotation = target.rotation;
        isMoving = false;


        
        if(animator!=null){
        animator.SetBool("IsMoving", false);
        }
        

        onArrival?.Invoke();
    }

    private float GetHorizontalDistance(Vector3 targetPosition)
    {
        Vector3 current = transform.position;

        Vector2 currentXZ = new Vector2(current.x, current.z);
        Vector2 targetXZ = new Vector2(targetPosition.x, targetPosition.z);

        return Vector2.Distance(currentXZ, targetXZ);
    }
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void UpdateAnimation(Vector3 direction)
    {
        if(animator==null)
            return;
        
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveZ", direction.z);
    }
}
