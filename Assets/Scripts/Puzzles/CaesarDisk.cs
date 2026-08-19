using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaesarDisk : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [Header("Disk Settings")]
    [SerializeField] private int letterCount = 26;

    private RectTransform rectTransform;
    private RectTransform parentRect;

    private float previousPointerAngle;

    private float StepAngle => 360/letterCount;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        previousPointerAngle = GetPointerAngle(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float currentPointerAngle = GetPointerAngle(eventData);
        
        float deltaAngle = Mathf.DeltaAngle(previousPointerAngle,currentPointerAngle);

        rectTransform.Rotate(0f,0f,deltaAngle);

        previousPointerAngle = currentPointerAngle;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToLetter();
    }

    private float GetPointerAngle(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, eventData.pressEventCamera, out localPoint);

        return Mathf.Atan2(localPoint.y, localPoint.x)*Mathf.Rad2Deg;
    }

    private void SnapToLetter()
    {
        float currentAngle = rectTransform.localEulerAngles.z;

        float snappedAngle = Mathf.Round(currentAngle/StepAngle)*StepAngle;
        rectTransform.localRotation = Quaternion.Euler(0f,0f,snappedAngle);
    }

}
