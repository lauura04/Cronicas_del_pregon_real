using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Vista normal")]
    [SerializeField] private Vector3 defaultOffset =
        new Vector3(0f, 2f, -4.5f);

    [SerializeField] private Vector3 defaultRotation =
        new Vector3(25f, 0f, 0f);

    [Header("Suavizado")]
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 desiredOffset;
    private Quaternion desiredRotation;

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
        if (target == null)
        {
            Debug.LogError("CameraFollow no tiene un objetivo asignado.");
            enabled = false;
            return;
        }

        transform.position = target.position + desiredOffset;
        transform.rotation = desiredRotation;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition =
            target.position + desiredOffset;

        float positionBlend =
            1f - Mathf.Exp(-followSpeed * Time.deltaTime);

        float rotationBlend =
            1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);

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
        desiredOffset = newOffset;
        desiredRotation = Quaternion.Euler(newRotation);
    }

    public void ResetCameraView()
    {
        desiredOffset = defaultOffset;
        desiredRotation = Quaternion.Euler(defaultRotation);
    }
}