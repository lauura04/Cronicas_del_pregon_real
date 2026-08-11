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

    private void Awake()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Show()
    {
        if (panel != null)
        {
            panel.SetActive(true);
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