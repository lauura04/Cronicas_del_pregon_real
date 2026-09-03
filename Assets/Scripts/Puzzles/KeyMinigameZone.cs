using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMinigameZone : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private bool firstTime = true;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(playerTag))
        {
            return;
        }
        if(firstTime)
        {
            firstTime = false;
            return;
        }
        if (KeyMinigameManager.Instance.MinigameCompleted)
        {
            return;
        }
        if(KeyMinigameManager.Instance != null)
        {
            KeyMinigameManager.Instance.StartMinigame();
        }
    }
}
