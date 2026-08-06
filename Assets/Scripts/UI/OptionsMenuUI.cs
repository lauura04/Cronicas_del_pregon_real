using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Paneles")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject previousPanel;

    private bool isInitializing;

    private void OnEnable()
    {
        InitializeMenu();
    }

    private void InitializeMenu()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError(
                "SettingsManager instance is null. " +
                "Cannot initialize options menu."
            );

            return;
        }

        if (volumeSlider == null || resolutionDropdown == null)
        {
            Debug.LogError(
                "El Slider de volumen o el Dropdown de resolución " +
                "no están asignados en OptionsMenuUI."
            );

            return;
        }

        isInitializing = true;

        ConfigureVolumeSlider();
        ConfigureResolutionDropdown();

        isInitializing = false;
    }

    private void ConfigureVolumeSlider()
    {
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.wholeNumbers = false;

        volumeSlider.SetValueWithoutNotify(
            SettingsManager.Instance.CurrentVolume
        );
    }

    private void ConfigureResolutionDropdown()
    {
        int resolutionIndex =
            SettingsManager.Instance.GetCurrentResolutionIndex();

        resolutionDropdown.SetValueWithoutNotify(resolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void OnVolumeChanged(float newVolume)
    {
        if (isInitializing)
        {
            return;
        }

        SettingsManager.Instance.SetVolume(newVolume);
    }

    public void OnResolutionChanged(int newResolutionIndex)
    {
        if (isInitializing)
        {
            return;
        }

        SettingsManager.Instance.SetResolution(newResolutionIndex);
    }

    public void ReturnToPreviousPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if (previousPanel != null)
        {
            previousPanel.SetActive(true);
        }
    }
}