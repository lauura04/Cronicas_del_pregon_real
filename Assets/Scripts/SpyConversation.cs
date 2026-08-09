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

    private int currentLineIndex;
    private string currentlyWrittenText="";

    private DialogueLine currentLine;

    public DialogueLine CurrentLine => currentLine;
    public string CurrentText => currentlyWrittenText;

    private void Start()
    {
        if (spyDialogue == null)
        {
            return;
        }
        StartCoroutine(ConversationRoutine());
    }

    private IEnumerator ConversationRoutine()
    {
        while (true)
        {
            currentLine = spyDialogue.Lines[currentLineIndex];
            currentlyWrittenText = "";

            foreach(char character in currentLine.Text)
            {
                currentlyWrittenText+=character;;

                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(pauseBetweenLines);
            currentLineIndex++;

            if (currentLineIndex >= spyDialogue.Lines.Count)
            {
                if (loopConversation)
                {
                    currentLineIndex = 0;
                }

                else
                {
                    yield break;
                }
            }
        }
    }

    public void SetDialogue(DialogueData newDialogue)
{
    if (spyDialogue == newDialogue)
    {
        return;
    }

    StopAllCoroutines();

    spyDialogue = newDialogue;

    currentLineIndex = 0;
    currentlyWrittenText = "";
    currentLine = null;

    if (spyDialogue == null ||
        spyDialogue.Lines == null ||
        spyDialogue.Lines.Count == 0)
    {
        return;
    }

    StartCoroutine(ConversationRoutine());
}
}
