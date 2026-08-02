using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite protrait;

    public string CharacterName => characterName;
    public Sprite Portrait => protrait;
}
