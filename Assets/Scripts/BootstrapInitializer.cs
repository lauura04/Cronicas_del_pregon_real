using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapInitializer : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}