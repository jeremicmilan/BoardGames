using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reversi : Game {

    public bool isAttack;
    private List<Piece> toChange = new List<Piece> ();

    public Reversi ()
        : base(GameName.REVERSI, "Reversi", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(8, 8, BoardType.UNCHECKERED);

        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE }},
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE }},
                              true);
    }
    public override void StartSinglePlayer () {
        SetBoardAndPieces();
        board.markerPossible = board.markerInvisible;
        board.markerSelected = board.markerInvisible;
        MarkFields(new Position(-1, -1, null), returnPossibleMoves());
        board.UpdatePlayerStatusText();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
        board.markerPossible = board.markerInvisible;
        board.markerSelected = board.markerInvisible;
        MarkFields(new Position(-1, -1, null), returnPossibleMoves());
        board.UpdatePlayerStatusText();
    }

    public override bool Attack (Move move, bool destroy = true) {
        return true;
    }

    public void ChangeColor(Position pos, Position direction) {
        Position currentPosition = pos + direction;

        if (!board.ValidPosition(currentPosition) || CanMoveTo(currentPosition))
            return;

        Piece piece = board.GetField(currentPosition).FindPiece();

        if (piece.isWhite != isWhitesTurn) {
            isAttack = true;
            toChange.Add(piece);
            ChangeColor(currentPosition, direction);
        }

        if (piece.isWhite == isWhitesTurn && isAttack) {
            foreach (Piece p in toChange) {
                board.setPiece(PieceType.RV_PAWN, p.position);
                board.SendToGraveyard(p);
            }
        }
    }


    public void ChangeColor(Position end) {
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(0, 1, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(0, -1, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, 0, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, 0, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, 1, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, -1, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, -1, null));
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, 1, null));
    }

    public List<Move> returnPossibleMoves () {
        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        List<Move> possibleMoves = new List<Move> ();

        foreach(Piece piece in pieces)
            if(piece.isWhite == isWhitesTurn)
                possibleMoves.AddRange(piece.GetPawnReversiMoves());


        return possibleMoves;
    }

    public override bool CanMakeMove(Move move) {
        List<Move> possibleMoves = returnPossibleMoves();
            foreach (Move m in possibleMoves)
                if (m.end == move.end)
                    return true;

        return false;
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override void MakeMove(Move move, bool fake = false) {

        ChangeColor(move.end);
        board.setPiece(PieceType.RV_PAWN, move.end);

        isWhitesTurn = !isWhitesTurn;

        if (returnPossibleMoves().Count == 0)
            isWhitesTurn = !isWhitesTurn;

        board.ClearMarkers();
        board.moveHistory.Push(move);
        board.UpdatePlayerStatusText();

        MarkFields(new Position(-1,-1,null), returnPossibleMoves());
    }

    public override void MarkFields(Position start, List<Move> possibleMoves) {
        board.ClearMarkers();

        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> moves = piece.PossibleMoves();
                foreach (Move move in moves) {
                    board.MakeMarker(move.end, board.markerAttack);
                }
            }
        }

    }

    public bool noMoreMoves() {
        bool result = false;
        if (returnPossibleMoves().Count == 0) {
            isWhitesTurn = !isWhitesTurn;
            if (returnPossibleMoves().Count == 0)
                result = true;
            isWhitesTurn = !isWhitesTurn;
        }

        return result;
    }

    public override bool CheckForEnd (ref bool? whiteWon) {
        int white = 0;
        int black = 0;
        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        if (pieces.Count == 64 || noMoreMoves()) {
            foreach (Piece piece in pieces) {
                if (piece.isWhite)
                    white++;
                else
                    black++;
            }

            if (white > black)
                whiteWon = true;
            else if (white < black)
                whiteWon = false;
            else
                whiteWon = null;

            return true;
        }

        return false;
    }

    public override Move getAIMove () {
        throw new NotImplementedException();
    }
}
