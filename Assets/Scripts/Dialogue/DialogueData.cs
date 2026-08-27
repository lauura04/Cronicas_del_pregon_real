using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] private CharacterData character;
    [SerializeField] [TextArea(2, 5)] private string text;

    [Header("Optional images")]
    [SerializeField] private Sprite leftImage;
    [SerializeField] private Sprite rightImage;

    public CharacterData Character => character;
    public string Text => text;

    public Sprite LeftImage => leftImage;
    public Sprite RightImage => rightImage;
}

[CreateAssetMenu(fileName="New Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private List<DialogueLine> lines;
    public IReadOnlyList<DialogueLine> Lines =>lines;
}
