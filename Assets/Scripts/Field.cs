using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum FieldType { BLACK, WHITE, CASTL, ESCAP, NEUTR }

public class Field : MonoBehaviour {

    public FieldType fieldType;

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
        if (board.game.gameEnded) {
            return;
        }

        Piece piece = FindPiece();
        Move move = new Move(board.previousPositionClicked, position);

        if (board.CanMakeMove(move)) {
            board.game.MakeMove(move);

            bool? whiteWon = false;
            if (board.game.CheckForEnd(ref whiteWon)) {
                Text status = GameObject.FindGameObjectWithTag("OnTheMove").GetComponent<Text>();

                if (whiteWon.HasValue)
                    status.text = (whiteWon.Value ? "White" : "Black") + " won!";
                else
                    status.text = "Draw!";
            }
            

        } else if (piece) {
            piece.OnClick();
        } else {
            transform.parent.GetComponent<Board>().MarkSelected(position);
        }

        board.previousPositionClicked = position;
    }
}
