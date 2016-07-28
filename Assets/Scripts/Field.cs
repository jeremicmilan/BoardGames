using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {

    [HideInInspector]
    public Position position;

    [HideInInspector]
    public Board board;

    public Piece FindPiece () {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Piece>()) {
                return transform.GetChild(i).GetComponent<Piece>();
            }
        }

        return null;
    }

	void OnMouseDown () {
        Piece piece = FindPiece();
        Move move = new Move(board.previousPositionClicked, position);

        if (piece) {
            piece.OnClick();
        } else if (board.CanMakeMove(move)) {
            board.MakeMove(move);
        } else {
            transform.parent.GetComponent<Board>().MarkSelected(position);
        }

        board.previousPositionClicked = position;
    }
}
