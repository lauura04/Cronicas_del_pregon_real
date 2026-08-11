using UnityEngine;
using UnityEngine.UI;

public class SpyUI : MonoBehaviour
{
    public static SpyUI Instance {get;private set;}

    [SerializeField] private GameObject spyPanel;
    [SerializeField] private Slider spySlider;

    private void Awake()
    {
        Instance = this;
        spyPanel.SetActive(false);
    }

    public void Show()
    {
        spySlider.value = 0f;
        spyPanel.SetActive(true);
    }

    public void SetProgress(float progress)
    {
        spySlider.value = Mathf.Clamp01(progress);
    }

    public void Hide()
    {
        spyPanel.SetActive(false);
        spySlider.value = 0f;
    }
}
