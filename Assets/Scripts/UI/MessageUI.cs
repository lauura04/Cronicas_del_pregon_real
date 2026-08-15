using System.Collections;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance { get; private set; }

    [Header("Interfaz")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        Debug.Log("AWAKE MESSAGE UI: " + gameObject.name);

        if (Instance != null && Instance != this)
        {
            Debug.Log("MESSAGE UI DUPLICADO, destruyendo: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        messagePanel.SetActive(false);
    }

    public void ShowMessage(string message, float duration = -1f)
    {
        Debug.Log("ShowMessage ejecutado con: " + message);
        messageText.text = message;
        messagePanel.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        if (duration > 0f)
        {
            hideCoroutine = StartCoroutine(
                HideAfterSeconds(duration)
            );
        }
    }

    public void HideMessage()
    {
        Debug.Log(
            "HIDEMESSAGE desde: " + gameObject.name +
            " | Instance: " + Instance?.gameObject.name
        );

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        messagePanel.SetActive(false);
        messageText.text = "";
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        HideMessage();
    }

     public void Close()
    {
        Debug.Log("CLOSE BUTTON");

        if (MessageUI.Instance != null)
        {
            MessageUI.Instance.HideMessage();
        }
        else
        {
            Debug.LogError("MessageUI.Instance es null");
        }
    }
}