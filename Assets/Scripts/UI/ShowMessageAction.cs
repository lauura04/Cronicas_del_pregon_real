using UnityEngine;
using System.Collections;

public class ShowMessageAction : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string message;

    public void Show()
    {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow()
    {
        while (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            yield return null;
        }

        MessageUI.Instance.ShowMessage(message);
    }



    public void Hide()
    {
        
        MessageUI.Instance.HideMessage();
    }
}