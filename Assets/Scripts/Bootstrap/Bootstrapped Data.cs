using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    const string SceneName = "BootstrapScene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void execute()
    {
        // traverse the currently loaded scenes
        for(int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);

            //sale si ya está cargada
            if(candidate.name == SceneName)
            {
                return;
            }
        }


        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}

public class BootstrappedDAta : MonoBehaviour
{
    public static BootstrappedDAta Instance { get; private set; } = null;
    void Awake()
    {
        //comprueba si ya existe una instancia --> SINGLETON
        if (Instance != null){
            Debug.LogError("Ya existe una instancia de BoostrappedData en" + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        //prevenir que se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }

    public void Test()
    {
        Debug.Log("BootstrappedData is working");
    }
}
