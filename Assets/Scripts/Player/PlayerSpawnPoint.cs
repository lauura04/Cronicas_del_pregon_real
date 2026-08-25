using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private bool setRotation = false;
    [SerializeField] private bool spawnOnStart = false;
    private void Start()
    {
        if (spawnOnStart)
        {
          TeleportPlayer();  
        }
        
    }

    public void TeleportPlayer()
    {
        if(PlayerMovement.Instance == null)
        {
            Debug.LogError("No existe playermovement");
            return;
        }

        PlayerMovement.Instance.TeleportTo(transform.position);
        if (setRotation)
        {
            PlayerMovement.Instance.transform.rotation = transform.rotation;
        }
    }
}
