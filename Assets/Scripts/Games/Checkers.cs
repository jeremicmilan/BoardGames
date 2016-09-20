using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers") { }

    protected override void SetBoardAndPieces () {
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

    protected override void GameSpecificStartSinglePlayer () {
        isWhitesTurn = false;
        isAIWhite = true;
    }

    protected override void GameSpecificStartTwoPlayer () {
        isWhitesTurn = false;
        isAIWhite = true;
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

    public override void GameSpecificPreMakeMove (ref Move move, Piece piece, bool attacked, bool fake) {
        if (CheckForPieceEvolve(move))
            move.pieceEvolved = true;

        if (CheckForAttack(piece) && attacked)
            isWhitesTurn = !isWhitesTurn;
    }

    public override void UndoMove(Move move, bool fake = false) {
        Piece piece = move.end.field.FindPiece();
        bool attacked = false;

        if (CheckForAttack(piece))
            attacked = true;

        base.UndoMove(move, fake);

        isWhitesTurn = !isWhitesTurn;

        if (!attacked || !CheckForAttack(piece))
            isWhitesTurn = !isWhitesTurn;

        if (move.pieceEvolved)
            PieceDevolve(move);

        if (!fake) {
            board.UpdatePlayerStatusText();
        }

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

        foreach (Move move in toRemove) {
            possibleMoves.Remove(move);
        }
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override bool CheckForEnd (ref bool? whiteWon, bool isWhitesTurn) {
        int white = 0;
        int black = 0;

        bool whiteCanMove = false;
        bool blackCanMove = false;

        List<Piece> pieces = board.GetPieces(PieceType.CK_PAWN).ToList();
        foreach(Piece piece in pieces)
        {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves(piece.isWhite).Any())
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves(piece.isWhite).Any())
                    blackCanMove = true;
            }
        }

        pieces = board.GetPieces(PieceType.CK_KING).ToList();
        foreach (Piece piece in pieces) {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves(piece.isWhite).Any())
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves(piece.isWhite).Any())
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
        foreach (Piece piece in board.GetPieces(PieceType.CK_PAWN)) {
            if (piece.isWhite == isWhitesTurn) {
                if ((isWhitesTurn && piece.position.y == 0) || (!isWhitesTurn && piece.position.y == board.height - 1)) {
                    move.eatenPieces.Add(piece);
                    board.SendToGraveyard(piece);
                    board.setPiece(PieceType.CK_KING, isWhitesTurn, move.end);
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

    public bool CheckForAttack(Piece piece) {
        foreach (Move move in piece.PossibleMoves(isWhitesTurn)) {
            if (move.isAttack) {
                return true;
            }
        }
        return false;
    }

    public bool CheckForAttack() {
        foreach(Piece piece in board.GetPieces(PieceType.CK_PAWN)) {
            if (piece.isWhite == isWhitesTurn) {
                foreach (Move move in piece.PossibleMoves(isWhitesTurn)) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }

        foreach (Piece piece in board.GetPieces(PieceType.CK_KING)) {
            if (piece.isWhite == isWhitesTurn) {
                foreach (Move move in piece.PossibleMoves(isWhitesTurn)) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
