using UnityEngine;

[CreateAssetMenu(fileName ="NewChapterPhase", menuName="Game/Chapter Phase")]

public class ChapterPhaseData : ScriptableObject
{
    [SerializeField] private string phaseName;
    public string PhaseName =>phaseName;
}
