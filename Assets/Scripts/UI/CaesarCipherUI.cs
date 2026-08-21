using UnityEngine;

public class CaesarCipherUI : MonoBehaviour
{
    public static CaesarCipherUI Instance {get; private set;}

    [SerializeField] private GameObject panel;

    private void Awake()
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(gameObject);
            
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
            panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(false);
        }
    }

    public void Close()
    {
        panel.SetActive(false);
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.SetMovementEnabled(true);
        }
    }
}
