using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        ai.board = board;
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(8, 8, BoardType.CHECKERED);


        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE } },
                              true);

    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
        board.UpdatePlayerStatusText();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
        board.UpdatePlayerStatusText();
    }

    public override bool Attack (Move move, bool destroy = true) {
        if (Math.Abs(move.start.x - move.end.x) == 2) {
            Piece piece = board.GetField(move.start.x + (move.end.x - move.start.x) / 2, move.start.y + (move.end.y - move.start.y) / 2).FindPiece();
            if (destroy) {
                move.eatenPieces.Add(piece);
                board.SendToGraveyard(piece);
            }
            return true;
        }


        return false;
    }

    public override void MakeMove(Move move, bool fake = false) {
        Piece piece = move.start.field.FindPiece();

        piece.transform.parent = move.end.field.transform;
        piece.position = move.end;
        piece.transform.localPosition = new Vector3(0, 0, -1);

        if (CheckForPieceEvolve(move))
            move.pieceEvolved = true;


        bool attacked = Attack(move);

        if (!CheckForAttack() || !attacked)
            isWhitesTurn = !isWhitesTurn;

        board.ClearMarkers();
        board.moveHistory.Push(move);
        board.UpdatePlayerStatusText();
    }

    public override void UndoMove(Move move, bool fake = false) {
        base.UndoMove(move);

        if (move.pieceEvolved)
            PieceDevolve(move);

    }

    public override void MarkFields(Position start, List<Move> possibleMoves) {
        board.ClearMarkers();

        board.MakeMarker(start, board.markerSelected);

        List<Move> toRemove = new List<Move>();

        foreach (Move move in possibleMoves) {
            if (Attack(move, false)) {
                board.MakeMarker(move.end, board.markerAttack);
            } else if (!CheckForAttack()) {
                board.MakeMarker(move.end, board.markerPossible);
            } else {
                toRemove.Add(move);
            }
        }

        possibleMoves.RemoveAll(x => toRemove.Contains(x));
    }

    public override bool CanMakeMove(Move move) {
        return board.previousPossibleMoves != null && board.previousPossibleMoves.Contains(move);
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override bool CheckForEnd (ref bool? whiteWon) {
        int white = 0;
        int black = 0;

        bool whiteCanMove = false;
        bool blackCanMove = false;

        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach(Piece piece in pieces)
        {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves().Count > 0)
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves().Count > 0)
                    blackCanMove = true;
            }
        }

        pieces = board.FindAllPieces(PieceType.CK_KING);
        foreach (Piece piece in pieces) {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves().Count > 0)
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves().Count > 0)
                    blackCanMove = true;
            }
        }

        if(white == 0 || (!whiteCanMove && isWhitesTurn)) {
            whiteWon = false;
            return true;
        }
        else if(black == 0 || (!blackCanMove && !isWhitesTurn)){
            whiteWon = true;
            return true;
        }

        return false;

    }

    public bool CheckForPieceEvolve (Move move) {
        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                if ((isWhitesTurn && piece.position.y == 0) || (!isWhitesTurn && piece.position.y == board.height - 1)) {
                    move.eatenPieces.Add(piece);
                    board.SendToGraveyard(piece);
                    board.setPiece(PieceType.CK_KING, move.end);
                    return true;
                }
            }
        }

        return false;
    }

    public void PieceDevolve(Move move) {
        board.SendToGraveyard(move.start.field.FindPiece());
        Piece piece = move.end.field.FindPiece();

        piece.transform.parent = move.start.field.transform;
        piece.position = move.start;
        piece.transform.localPosition = new Vector3(0, 0, -1);
        board.ClearMarkers();

    }

    public bool CheckForAttack() {

        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach(Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> possibleMoves = piece.PossibleMoves();
                foreach (Move move in possibleMoves) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }

        pieces = board.FindAllPieces(PieceType.CK_KING);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> possibleMoves = piece.PossibleMoves();
                foreach (Move move in possibleMoves) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }


        return false;
    }

    public override Move getAIMove () {
        throw new NotImplementedException();
    }

    public override int scoreBoard () {
        throw new NotImplementedException();
    }
}
