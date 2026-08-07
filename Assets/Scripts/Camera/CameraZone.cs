using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    public enum CameraMode
    {
        Follow,
        Fixed
    }

    [Header("Modo")]
    [SerializeField] private CameraMode cameraMode =
        CameraMode.Follow;

    [Header("Vista Follow")]
    [SerializeField] private Vector3 cameraOffset =
        new Vector3(0f, 3f, -5f);

    [SerializeField] private Vector3 cameraRotation =
        new Vector3(45f, 0f, 0f);

    [Header("Vista fija")]
    [SerializeField] private Transform fixedCameraPoint;

    [Header("Salida")]
    [SerializeField] private bool resetOnExit = true;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (CameraFollow.Instance == null)
        {
            return;
        }

        if (cameraMode == CameraMode.Fixed)
        {
            if (fixedCameraPoint == null)
            {
                Debug.LogWarning(
                    $"No hay Fixed Camera Point en {gameObject.name}."
                );

                return;
            }

            CameraFollow.Instance.SetFixedCameraView(
                fixedCameraPoint.position,
                fixedCameraPoint.eulerAngles,
                this
            );
        }
        else
        {
            CameraFollow.Instance.SetCameraView(
                cameraOffset,
                cameraRotation
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (CameraFollow.Instance == null)
        {
            return;
        }

        if (cameraMode == CameraMode.Fixed)
        {
            CameraFollow.Instance.ExitFixedCameraView(this);
            return;
        }

        if (resetOnExit)
        {
            CameraFollow.Instance.ResetCameraView();
        }
    }
}