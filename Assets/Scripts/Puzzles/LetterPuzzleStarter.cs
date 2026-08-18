using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPuzzleStarter : MonoBehaviour
{
    [SerializeField] private LetterPuzzleData puzzleData;

    public void StartPuzzle()
    {
        if(LetterPuzzleManager.Instance == null)
        {
            Debug.LogError("LetterPuzzleManager no existe");
            return;
        }

        LetterPuzzleManager.Instance.StartPuzzle(puzzleData);
    }
}
