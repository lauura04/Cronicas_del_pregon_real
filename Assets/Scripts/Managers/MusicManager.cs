using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public static MusicManager Instance {get; private set;}
   [Header("Audio")]
   [SerializeField] private AudioSource audioSource;
   [Header("Fade")]
   [SerializeField] private float fadeDuration = 1f;

   private AudioClip previousClip;
   private float previousVolume;

   private Coroutine fadeCoroutine;

   private void Awake()
    {
        if(Instance !=null && Instance != this)
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

    public void PlayMusic(AudioClip clip)
    {
        if(clip==null)
            return;
        if(audioSource.clip == clip && audioSource.isPlaying)
            return;
        ChangeMusic(clip);
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    public void PlayTemporaryMusic(AudioClip temporaryClip)
    {
        if(temporaryClip == null)
            return;
        previousClip = audioSource.clip;
        previousVolume = audioSource.volume;

        ChangeMusic(temporaryClip);
    }

    public void RestorePreviousMusic()
    {
        if(previousClip==null)
            return;
        AudioClip clipToRestore = previousClip;
        previousClip= null;
        ChangeMusic(clipToRestore);
    }

    private void ChangeMusic(AudioClip newClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToNewMusic(newClip));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        while(audioSource.volume > 0f)
        {
            audioSource.volume-= startVolume*Time.unscaledDeltaTime/fadeDuration;
            yield return null;
        }

        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.loop = true;
        audioSource.Play();

        while(audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume*Time.unscaledDeltaTime/fadeDuration;
            yield return null;
        }

        audioSource.volume = startVolume;
        fadeCoroutine = null;
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;
        while(audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime/fadeDuration;
            yield return null;
        }

        audioSource.Stop();

        audioSource.volume = startVolume;
        fadeCoroutine = null;
    }
}
