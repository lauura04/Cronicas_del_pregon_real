using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FinalSceneController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Scene")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool wasPaused;

    private void Start()
    {
        HUDManager.Instance.HideInventoryButton();

        if (videoPlayer == null)
        {
            Debug.LogError(
                "FinalSceneController: VideoPlayer no asignado."
            );
            return;
        }

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    private void Update()
    {
        if (videoPlayer == null)
            return;

        bool isPaused = Time.timeScale == 0f;

        if (isPaused && !wasPaused)
        {
            videoPlayer.Pause();
        }
        else if (!isPaused && wasPaused)
        {
            videoPlayer.Play();
        }

        wasPaused = isPaused;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        // Por seguridad, por si terminamos estando pausados
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}