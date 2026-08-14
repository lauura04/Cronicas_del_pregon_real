using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewChapter", menuName = "Game/Chapter")]


public class ChapterData : ScriptableObject
{
    [SerializeField] private GameChapter chapter;
    [SerializeField] private List<ChapterPhaseData> phases;

    public GameChapter Chapter => chapter;
    public IReadOnlyList<ChapterPhaseData> Phases => phases;

    public bool ContainsPhase(ChapterPhaseData phase)
    {
        return phases.Contains(phase);
    }
   public int GetPhaseIndex(ChapterPhaseData phase)
{
    if (phases == null || phase == null)
    {
        return -1;
    }

    return phases.IndexOf(phase);
}
}
