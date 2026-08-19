using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Slider volumeSlider;
   

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

        if (volumeSlider == null )
        {
            Debug.LogError(
                "El Slider de volumen no está asignado en OptionsMenuUI."
            );

            return;
        }

        isInitializing = true;

        ConfigureVolumeSlider();

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

    

    public void OnVolumeChanged(float newVolume)
    {
        if (isInitializing)
        {
            return;
        }

        SettingsManager.Instance.SetVolume(newVolume);
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