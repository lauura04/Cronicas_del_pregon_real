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

    [Header("Controles")]
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = false;
    [SerializeField] private bool swapAxes = false;

    [Header("Player")]
    [SerializeField] private bool swapFrontBackSprites = false;
    [SerializeField] private bool swapRightLeftSprites = false;

    [SerializeField] private bool changePlayerRotation = false;
    [SerializeField] private Vector3 playerRotation;
    private static readonly Vector3 defaultPlayerRotation = new Vector3(0f, 0f, 0f);

    [Header("Salida")]
    [SerializeField] private bool resetOnExit = true;

    // La última CameraZone que ha aplicado controles
    private static CameraZone activeControlZone;

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

        // Esta zona pasa a controlar el mapping del jugador
        activeControlZone = this;

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementMapping(
                invertHorizontal,
                invertVertical,
                swapAxes
            );

            PlayerMovement.Instance.SetSpriteMapping(
                swapFrontBackSprites, swapRightLeftSprites
            );
            if (changePlayerRotation)
    {
        PlayerMovement.Instance.SetGraphicsRotation(playerRotation);
    }
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

        // Solo esta zona puede resetear si sigue siendo
        // la CameraZone activa.
        if (activeControlZone == this)
        {
            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.ResetMovementMapping();
                PlayerMovement.Instance.ResetSpriteMapping();
                PlayerMovement.Instance.ResetGraphicsRotation();
            }

            activeControlZone = null;
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