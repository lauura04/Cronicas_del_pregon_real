using UnityEngine;

public class KeyMinigameStarter : MonoBehaviour
{
  public void StartMinigame()
    {
        if (KeyMinigameManager.Instance != null)
        {
            KeyMinigameManager.Instance.StartMinigame();
        }
        else
        {
            Debug.LogError("No existe KeyMinigameManager");
        }
    }
}
