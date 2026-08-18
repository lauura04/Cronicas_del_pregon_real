using UnityEngine;
 [CreateAssetMenu(fileName="NewLetterPuzzle", menuName="Game/Letter Puzzle Data")]
public class LetterPuzzleData : ScriptableObject
{
  [Header("Identification")]
  public string puzzleName;

  [Header("Puzzle")]
  [TextArea(3,10)]
  public string correctText;

  [Header("Visual")]
  public Sprite letterBackground;
}
