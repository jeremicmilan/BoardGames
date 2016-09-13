using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Game {

    public GameName gameName;
    public string name;
    public string description;
    public GameObject picture;

    public Board board;

    public bool isWhitesTurn;
    public bool gameEnded = false;

    public Game (GameName gameName, string name, string description, GameObject picture) {
        this.gameName = gameName;
        this.name = name;
        this.description = name;
        this.picture = picture;
    }

    public abstract void StartSinglePlayer ();
    public abstract void StartTwoPlayer ();

    public abstract bool Attack (Move move, bool destroy = true);
    public abstract void MarkFields(Position start, List<Move> possibleMoves);
    public abstract void MakeMove(Move move, bool fake = false);

    public virtual void UndoMove(Move move, bool fake = false) {
        Piece piece = move.end.field.FindPiece();

        piece.transform.parent = move.start.field.transform;
        piece.position = move.start;

        if (!fake) {
            piece.transform.localPosition = new Vector3(0, 0, -1);
            board.ClearMarkers();
        }

        isWhitesTurn = !isWhitesTurn;

        gameEnded = false;

        foreach (Piece p in move.eatenPieces) {
            p.transform.parent = p.position.field.transform;
            p.transform.localPosition = new Vector3(0, 0, -1);
        }

        if (!fake) {
            board.UpdatePlayerStatusText();
        }
    }

    public virtual List<Move> EliminateImpossibleMoves (List<Move> moves) {
        return moves;
    }

    public abstract bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE);
    public abstract bool CanMakeMove(Move move);

    public bool CanMoveTo (Position position, PieceType pieceType = PieceType.AL_NONE) {
        return CanMoveTo(position.x, position.y, pieceType);
    }

    public abstract bool CheckForEnd (ref bool? whiteWon);

    public abstract Move getAIMove ();
    public int minimax (Field[,] board) {

        return 0;
    }
}
