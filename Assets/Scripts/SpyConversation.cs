using System.Collections;
using UnityEngine;

public class SpyConversation : MonoBehaviour
{
    [Header("Conversation")]
    [SerializeField] private DialogueData spyDialogue;

    [Header("Times")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 1f;

    [Header("Behaviour")]
    [SerializeField] private bool loopConversation = true;

    private int currentLineIndex = 0;
    private string currentlyWrittenText = "";
    private DialogueLine currentLine;

    private Coroutine conversationCoroutine;

    public DialogueLine CurrentLine => currentLine;
    public string CurrentText => currentlyWrittenText;

    private void OnEnable()
    {
        RestartInternalConversation();
    }

    private void OnDisable()
    {
        if (conversationCoroutine != null)
        {
            StopCoroutine(conversationCoroutine);
            conversationCoroutine = null;
        }

        currentLine = null;
        currentlyWrittenText = "";
    }

    private void RestartInternalConversation()
    {
        if (conversationCoroutine != null)
        {
            StopCoroutine(conversationCoroutine);
            conversationCoroutine = null;
        }

        currentLineIndex = 0;
        currentlyWrittenText = "";
        currentLine = null;

        if (spyDialogue == null ||
            spyDialogue.Lines == null ||
            spyDialogue.Lines.Count == 0)
        {
            return;
        }

        conversationCoroutine = StartCoroutine(ConversationRoutine());
    }

    private IEnumerator ConversationRoutine()
    {
        while (true)
        {
            currentLine = spyDialogue.Lines[currentLineIndex];
            currentlyWrittenText = "";

            foreach (char character in currentLine.Text)
            {
                currentlyWrittenText += character;

                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            yield return new WaitForSecondsRealtime(pauseBetweenLines);

            currentLineIndex++;

            if (currentLineIndex >= spyDialogue.Lines.Count)
            {
                if (loopConversation)
                {
                    currentLineIndex = 0;
                }
                else
                {
                    conversationCoroutine = null;
                    yield break;
                }
            }
        }
    }

    public void SetDialogue(DialogueData newDialogue)
    {
        spyDialogue = newDialogue;

        if (gameObject.activeInHierarchy)
        {
            RestartInternalConversation();
        }
    }

    public void RestartConversation()
    {
        if (gameObject.activeInHierarchy)
        {
            RestartInternalConversation();
        }
    }
}