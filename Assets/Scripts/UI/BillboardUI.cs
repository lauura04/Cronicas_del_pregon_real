using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if(mainCamera==null)
            return;
        
        transform.rotation = mainCamera.transform.rotation;
    }
}
