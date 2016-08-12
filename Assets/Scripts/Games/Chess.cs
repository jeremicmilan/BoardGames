using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Chess : Game {

    public bool check = false;

    public Chess ()
        : base(GameName.CHESS, "Chess", "", null) { }

    void SetBoardAndPieces() {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(8, 8, BoardType.CHECKERED);


        board.SetPieces(new PieceType[,] { { PieceType.CH_ROOK, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_KNIG, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_BISH, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_QUEE, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_KING, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_BISH, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_KNIG, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CH_ROOK, PieceType.CH_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_ROOK },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_KNIG },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_BISH },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_QUEE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_KING },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_BISH },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_KNIG },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_PAWN, PieceType.CH_ROOK } },
                              true);
    }

    public override void StartSinglePlayer() {
        SetBoardAndPieces();
        isWhitesTurn = true;
        board.UpdateStatusText();
    }

    public override void StartTwoPlayer() {
        SetBoardAndPieces();
        isWhitesTurn = true;
        board.UpdateStatusText();
    }

    public override bool Attack(Move move, bool destroy = true) {
        Piece piece = board.GetField(move.end).FindPiece();
        if (piece) {
            if (destroy) {
                move.eatenPieces.Add(piece);
                board.SendToGraveyard(piece);
            }
            return true;
        }

        return false;
    }

    public override void MakeMove(Move move) {
        Piece piece = move.start.field.FindPiece();

        Attack(move);

        piece.transform.parent = move.end.field.transform;
        piece.position = move.end;
        piece.transform.localPosition = new Vector3(0, 0, -1);

        board.ClearMarkers();
        board.moveHistory.Push(move);

        if (stilCheck(move))
            return;

        isCheck();

        isWhitesTurn = !isWhitesTurn;

        if (check)
            board.UpdateStatusText("CHECK!");
        else
            board.UpdateStatusText();       
    }

    public bool stilCheck(Move move) {
        bool result = false;
        if (check) {
            isWhitesTurn = !isWhitesTurn;
            isCheck();
            if (check) {
                UndoMove(move);
                board.UpdateStatusText("CHECK!");
                result = true;
            } else {
                isWhitesTurn = !isWhitesTurn;
            }
        }       

        return result;
    }

    public void isCheck() {
        check = false;
        List<Piece> pieces = board.FindAllPieces(isWhitesTurn);

        foreach (Piece piece in pieces)
            piece.PossibleMoves();
    }

    public override void MarkFields(Position start, List<Move> possibleMoves) {
        board.ClearMarkers();

        board.MakeMarker(start, board.markerSelected);

        foreach (Move move in possibleMoves) {
            if (Attack(move, false)) {
                board.MakeMarker(move.end, board.markerAttack);
            } else {
                board.MakeMarker(move.end, board.markerPossible);
            }
        }
    }

    public override bool CanMoveTo(int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override bool CanMakeMove(Move move) {
        return board.previousPossibleMoves != null && board.previousPossibleMoves.Contains(move);
    }
    
    public override bool CheckForEnd(ref bool? whiteWon) {
        return false;
    }

    public override Move getAIMove() {
        throw new NotImplementedException();
    }
}
