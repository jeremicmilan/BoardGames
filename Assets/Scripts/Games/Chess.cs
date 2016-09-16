using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Chess : Game {
    public Chess ()
        : base(GameName.CHESS, "Chess", "", null) { }

    void SetBoardAndPieces() {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        ai.board = board;
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
        board.UpdatePlayerStatusText();
        GameObject.FindGameObjectWithTag("game status").GetComponent<Text>().text = "";
        playingAgainstAI = true;
    }

    public override void StartTwoPlayer() {
        SetBoardAndPieces();
        isWhitesTurn = true;
        board.UpdatePlayerStatusText();
        GameObject.FindGameObjectWithTag("game status").GetComponent<Text>().text = "";
        playingAgainstAI = false;
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

    public override void MakeMove(Move move, bool fake = false) {
        Piece piece = move.start.field.FindPiece();
        Assert.IsNotNull(piece);

        Attack(move);

        piece.transform.parent = move.end.field.transform;
        piece.position = move.end;
        piece.transform.localPosition = new Vector3(0, 0, -1);

        if (!fake) {
            board.ClearMarkers();
            if (CheckForPieceEvolve(move))
                move.pieceEvolved = true;
        }

        board.moveHistory.Push(move);

        if (!fake)
            board.UpdateGameStatusText(isCheck() ? "CHECK!" : "");

        isWhitesTurn = !isWhitesTurn;

        if (!fake)
            board.UpdatePlayerStatusText();

        if (playingAgainstAI && !fake && !isWhitesTurn)
            MakeMove(getAIMove());
    }

    public override void UndoMove (Move move, bool fake = false) {
        base.UndoMove(move, fake);

        if (move.pieceEvolved)
            PieceDevolve(move);

        isWhitesTurn = !isWhitesTurn;

        if (isCheck())
            board.UpdateGameStatusText("CHECK!");

        isWhitesTurn = !isWhitesTurn;
    }

    public override List<Move> EliminateImpossibleMoves (List<Move> moves) {
        List<Move> possibleMoves = new List<Move>();

        foreach (Move move in moves) {
            MakeMove(move, fake : true);

            if (!isCheck()) {
                possibleMoves.Add(move);
            }

            board.Undo(fake : true);
        }

        return possibleMoves;
    }

    public bool isCheck () {
        foreach (Move move in board.GetAllMoves(isWhitesTurn, eliminateMoves: false)) {
            Piece endPiece = board.GetPiece(move.end);
            if (endPiece && endPiece.pieceType == PieceType.CH_KING) {
                return true;
            }
        }
        return false;
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
        return !board.GetField(x, y).FindPiece();
    }

    public override bool CanMakeMove(Move move) {
        return board.previousPossibleMoves != null && board.previousPossibleMoves.Contains(move);
    }

    public bool CheckForPieceEvolve(Move move) {
        List<Piece> pieces = board.FindAllPieces(PieceType.CH_PAWN);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                if ((isWhitesTurn && piece.position.y == 0) || (!isWhitesTurn && piece.position.y == board.height - 1)) {
                    move.eatenPieces.Add(piece);
                    board.SendToGraveyard(piece);
                    board.setPiece(PieceType.CH_QUEE, move.end);
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

    public override bool CheckForEnd(ref bool? whiteWon) {
        if (board.GetAllMoves(isWhitesTurn).Count == 0) {
            isWhitesTurn = !isWhitesTurn;

            if (isCheck())
                whiteWon = isWhitesTurn;
            else
                whiteWon = null;

            isWhitesTurn = !isWhitesTurn;

            return true;
        }
        return false;
    }

    public override Move getAIMove() {
        return ai.minimax();
    }

    public override int scoreBoard () {
        return new System.Random().Next();
    }
}
