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

    [Header("Controles de movimiento")]
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = false;
    [SerializeField] private bool swapAxes = false;

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
         //controles
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementMapping(invertHorizontal,invertVertical,swapAxes);
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
       if(PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.ResetMovementMapping();
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