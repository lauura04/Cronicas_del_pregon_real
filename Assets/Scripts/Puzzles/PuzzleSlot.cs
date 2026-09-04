using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot:MonoBehaviour, IDropHandler
{
    [SerializeField] private int correctPieceID;
    private bool occupied;

    public void OnDrop(PointerEventData eventData)
    {
        PuzzlePiece piece = eventData.pointerDrag?.GetComponent<PuzzlePiece>();

        if (piece != null)
        {
            TryPlacePiece(piece);
        }
    }

    public bool TryPlacePiece(PuzzlePiece piece)
    {
        if (occupied)
        {
            return false;
        }

        if(piece.PieceID != correctPieceID)
        {
            return false;
        }

        occupied = true;

        piece.PlaceInSlot(transform);

        PuzzleManager.Instance.PiecePlaced();

        return true;
    }
}