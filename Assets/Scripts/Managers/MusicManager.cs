using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1f;

    private AudioClip previousClip;
    private float previousVolume;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // =========================
    // REPRODUCIR MÚSICA
    // =========================

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        // Si ya está sonando esta canción, no hacemos nada
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        ChangeMusic(clip);
    }

    // =========================
    // PARAR MÚSICA
    // =========================

    public void StopMusic()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    // =========================
    // PAUSAR / REANUDAR
    // Para los diálogos
    // =========================

    public void PauseMusic()
    {
        if (audioSource == null)
            return;

        // Si hay un fade activo, lo detenemos
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource == null)
            return;

        // UnPause continúa exactamente
        // desde el momento en el que se pausó
        audioSource.UnPause();
    }

    // =========================
    // MÚSICA TEMPORAL
    // =========================

    public void PlayTemporaryMusic(AudioClip temporaryClip)
    {
        if (temporaryClip == null)
            return;

        previousClip = audioSource.clip;
        previousVolume = audioSource.volume;

        ChangeMusic(temporaryClip);
    }

    public void RestorePreviousMusic()
    {
        if (previousClip == null)
            return;

        AudioClip clipToRestore = previousClip;

        previousClip = null;

        ChangeMusic(clipToRestore);
    }

    // =========================
    // CAMBIO DE MÚSICA
    // =========================

    private void ChangeMusic(AudioClip newClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToNewMusic(newClip));
    }

    // =========================
    // FADE ENTRE CANCIONES
    // =========================

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -=
                startVolume * Time.unscaledDeltaTime / fadeDuration;

            yield return null;
        }

        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.loop = true;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume +=
                startVolume * Time.unscaledDeltaTime / fadeDuration;

            yield return null;
        }

        audioSource.volume = startVolume;
        fadeCoroutine = null;
    }

    // =========================
    // FADE Y STOP
    // =========================

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -=
                startVolume * Time.unscaledDeltaTime / fadeDuration;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        fadeCoroutine = null;
    }
}