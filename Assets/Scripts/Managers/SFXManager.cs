using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance {get; private set;}

    [SerializeField] private AudioSource audioSource;

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

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if(clip==null)
            return;
        audioSource.PlayOneShot(clip,volume);
    }

    public void StopSFX()
    {
        audioSource.Stop();
    }
}
