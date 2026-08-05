using UnityEngine;
using UnityEngine.Audio;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(3840, 2160)
    };

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    private const string VolumeKey = "MasterVolume";
    private const string ResolutionKey = "ResolutionIndex";

    private const float DefaultVolume = 1f;
    private const float MinimumDecibels = -80f;

    public float CurrentVolume { get; private set; }

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
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);
        int savedResolution = PlayerPrefs.GetInt(ResolutionKey, 0);

        SetVolume(savedVolume, false);
        ApplyResolution(savedResolution, false);
    }

    public void SetVolume(float linearVolume)
    {
        SetVolume(linearVolume, true);
    }

    private void SetVolume(float linearVolume, bool saveSettings)
    {
        CurrentVolume = Mathf.Clamp01(linearVolume);

        float volumeInDecibels = CurrentVolume <= 0.0001f ? MinimumDecibels : Mathf.Log10(CurrentVolume) * 20f;

        if (audioMixer == null)
        {
            Debug.LogError(
                "No se ha asignado el AudioMixer en SettingsManager."
            );

            return;
        }

        bool parameterFound = audioMixer.SetFloat("MasterVolume", volumeInDecibels);

        if (!parameterFound)
        {
            Debug.LogWarning("MasterVolume parameter not found in AudioMixer.");
        }

        if (!saveSettings) return;

        PlayerPrefs.SetFloat(VolumeKey, CurrentVolume);
        PlayerPrefs.Save();
    }
    public void SetResolution(int index)
    {
        ApplyResolution(index, true);
    }

    private void ApplyResolution(int index, bool save)
    {
        index = Mathf.Clamp(index, 0, resolutions.Length - 1);

        Vector2Int resolution = resolutions[index];

        Screen.SetResolution(
            resolution.x,
            resolution.y,
            Screen.fullScreenMode
        );

        if (save)
        {
            PlayerPrefs.SetInt(ResolutionKey, index);
            PlayerPrefs.Save();
        }
    }

    public int GetCurrentResolutionIndex()
    {
        int savedIndex = PlayerPrefs.GetInt(ResolutionKey, 0);

    return Mathf.Clamp(
        savedIndex,
        0,
        resolutions.Length - 1
    );
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
