using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapInitializer : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private bool debugStartChapter1 = false;

    private void Start()
    {
        if (debugStartChapter1)
    {
        SceneLoader.Instance.LoadScene("Chap_1");
    }
    else
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }
    }
}