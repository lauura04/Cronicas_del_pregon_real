using UnityEngine;
using UnityEngine.UI;

public class CharmMinigameUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Moving Circle")]
    [SerializeField] private RectTransform movingCircle;

    [Header("Circle Image")]
    [SerializeField] private Image movingCircleImage;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color successColor = Color.green;

    

    public void Show()
    {
        if (panel == null)
        {
            Debug.LogError("CHARM UI: PANEL ES NULL");
            return;
        }

        Debug.Log($"CHARM PANEL: {panel.name}");
        Debug.Log($"ANTES - activeSelf: {panel.activeSelf}");
        Debug.Log($"ANTES - activeInHierarchy: {panel.activeInHierarchy}");

        panel.SetActive(true);

        Debug.Log($"DESPUÉS - activeSelf: {panel.activeSelf}");
        Debug.Log($"DESPUÉS - activeInHierarchy: {panel.activeInHierarchy}");

        if (panel.transform.parent != null)
        {
            Debug.Log(
                $"PADRE: {panel.transform.parent.name} | " +
                $"activo: {panel.transform.parent.gameObject.activeInHierarchy}"
            );
        }
    }

    public void Hide()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void SetCircleScale(float scale)
    {
        if (movingCircle == null)
        {
            return;
        }

        movingCircle.localScale =
            new Vector3(
                scale,
                scale,
                1f
            );
    }

    public void SetSuccessState(bool success)
    {
        if (movingCircleImage == null)
        {
            return;
        }

        movingCircleImage.color =
            success
                ? successColor
                : normalColor;
    }
}