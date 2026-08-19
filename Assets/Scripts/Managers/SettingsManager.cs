using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioMixer mainAudioMixer;

    private const string VolumeKey = "MasterVolume";

    public float CurrentVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    private void LoadSettings()
    {
        CurrentVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        ApplyVolume(CurrentVolume);
    }

    public void SetVolume(float volume)
    {
        CurrentVolume = Mathf.Clamp01(volume);

        ApplyVolume(CurrentVolume);

        PlayerPrefs.SetFloat(VolumeKey, CurrentVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float volume)
    {

        if (volume <= 0.0001f)
        {
            mainAudioMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            float volumeDB = Mathf.Log10(volume) * 20f;
            mainAudioMixer.SetFloat("MasterVolume", volumeDB);
        }
    }


}