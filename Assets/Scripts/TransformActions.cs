
using UnityEngine;

public class TransformActions : MonoBehaviour
{
    public void MoveTo(Transform target)
    {
        transform.SetPositionAndRotation(
            target.position,
            target.rotation
        );
    }
}
