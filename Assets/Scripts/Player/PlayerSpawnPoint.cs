using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private bool setRotation = false;
    private void Start()
    {
        if (PlayerMovement.Instance == null)
        {
            Debug.LogError("No existe el PlayerMovement.Instance");
            return;
        }
        PlayerMovement.Instance.TeleportTo(transform.position);
    }
}
