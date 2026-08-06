using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] private CharacterData character;
    [SerializeField] [TextArea(2, 5)] private string text;

    public CharacterData Character => character;
    public string Text => text;
}

[CreateAssetMenu(fileName="New Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private List<DialogueLine> lines;
    public IReadOnlyList<DialogueLine> Lines =>lines;
}
