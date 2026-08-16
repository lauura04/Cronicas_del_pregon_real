using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Vista normal")]
    [SerializeField] private Vector3 defaultOffset =
        new Vector3(0f, 3f, -5f);

    [SerializeField] private Vector3 defaultRotation =
        new Vector3(45f, 0f, 0f);

    [Header("Suavizado")]
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 desiredOffset;
    private Quaternion desiredRotation;

    private bool useFixedPosition;
    private Vector3 fixedPosition;

    // Guarda qué zona ha fijado la cámara
    private CameraZone activeFixedZone;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        desiredOffset = defaultOffset;
        desiredRotation = Quaternion.Euler(defaultRotation);
    }

    private void Start()
    {
        if (PlayerMovement.Instance != null)
    {
        target = PlayerMovement.Instance.transform;
    }
        ResetCameraImmediate();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition;

        if (useFixedPosition)
        {
            // NO depende del jugador
            desiredPosition = fixedPosition;
        }
        else
        {
            // Seguimiento normal
            desiredPosition =
                target.position + desiredOffset;
        }

        float positionBlend =
            1f - Mathf.Exp(
                -followSpeed * Time.deltaTime
            );

        float rotationBlend =
            1f - Mathf.Exp(
                -rotationSpeed * Time.deltaTime
            );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionBlend
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationBlend
        );
    }

    public void SetCameraView(
        Vector3 newOffset,
        Vector3 newRotation)
    {
        // Si hay una cámara fija activa,
        // las demás zonas no pueden tocarla.
        if (useFixedPosition)
        {
            return;
        }

        desiredOffset = newOffset;
        desiredRotation =
            Quaternion.Euler(newRotation);
    }

    public void ResetCameraView()
    {
        // Una zona normal tampoco puede
        // cancelar una cámara fija.
        if (useFixedPosition)
        {
            return;
        }

        desiredOffset = defaultOffset;
        desiredRotation =
            Quaternion.Euler(defaultRotation);
    }

    public void SetFixedCameraView(
        Vector3 position,
        Vector3 rotation,
        CameraZone zone)
    {
        useFixedPosition = true;

        fixedPosition = position;
        desiredRotation =
            Quaternion.Euler(rotation);

        activeFixedZone = zone;
    }

    public void ExitFixedCameraView(CameraZone zone)
    {
        // Solo la zona que fijó la cámara
        // puede liberarla.
        if (!useFixedPosition)
        {
            return;
        }

        if (activeFixedZone != zone)
        {
            return;
        }

        useFixedPosition = false;
        activeFixedZone = null;

        desiredOffset = defaultOffset;
        desiredRotation =
            Quaternion.Euler(defaultRotation);
    }

    private void ResetCameraImmediate()
    {
        if (target == null)
        {
            Debug.LogError(
                "CameraFollow no tiene un objetivo asignado."
            );

            enabled = false;
            return;
        }

        useFixedPosition = false;
        activeFixedZone = null;

        desiredOffset = defaultOffset;
        desiredRotation =
            Quaternion.Euler(defaultRotation);

        transform.position =
            target.position + defaultOffset;

        transform.rotation =
            desiredRotation;
    }
}