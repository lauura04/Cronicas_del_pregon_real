using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    [Header("Vista de esta zona")]
    [SerializeField] private Vector3 cameraOffset =
        new Vector3(0f, 2f, -4f);

    [SerializeField] private Vector3 cameraRotation =
        new Vector3(45f, 0f, 0f);

    [Header("Salida")]
    [SerializeField] private bool resetOnExit = true;

    private void Reset()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (CameraFollow.Instance == null)
        {
            Debug.LogWarning(
                "No se ha encontrado CameraFollow.Instance."
            );

            return;
        }

        CameraFollow.Instance.SetCameraView(
            cameraOffset,
            cameraRotation
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!resetOnExit || CameraFollow.Instance == null)
        {
            return;
        }

        CameraFollow.Instance.ResetCameraView();
    }
}