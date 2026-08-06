using UnityEngine;

public class ShowMessageAction : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string message;

    public void Show()
    {
        MessageUI.Instance.ShowMessage(message);
    }

    public void Hide()
    {
        MessageUI.Instance.HideMessage();
    }
}