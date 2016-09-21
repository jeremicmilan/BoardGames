using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chess : Game {
    public Chess ()
        : base(GameName.CHESS, "Chess") { }

    protected override void SetBoardAndPieces() {
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

    protected override void GameSpecificStartSinglePlayer () {
        isWhitesTurn = true;
        isAIWhite = false;
    }

    protected override void GameSpecificStartTwoPlayer () {
        isWhitesTurn = true;
        isAIWhite = false;
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

    public override void GameSpecificPreMakeMove (ref Move move, Piece piece, bool attacked, bool fake) {
        if (!fake && CheckForPieceEvolve(move))
            move.pieceEvolved = true;

        if (!fake)
            board.UpdateGameStatusText(isCheck(isWhitesTurn) ? "CHECK!" : "");
    }

    public override void UndoMove (Move move, bool fake = false) {
        base.UndoMove(move, fake);

        if (move.pieceEvolved)
            PieceDevolve(move);

        if (isCheck(!isWhitesTurn))
            board.UpdateGameStatusText("CHECK!");
    }

    public override bool isMovePossible (Move move, bool isWhitesTurn) {
        MakeMove(move, fake : true);

        bool isPossible = false;
        if (!isCheck(!isWhitesTurn))
            isPossible = true;

        board.Undo(fake : true);

        return isPossible;
    }

    public bool isCheck (bool isWhitesTurn) {
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

    public bool CheckForPieceEvolve(Move move) {
        foreach (Piece piece in board.GetPieces(isWhitesTurn, PieceType.CH_PAWN)) {
            if ((isWhitesTurn && piece.position.y == 0) || (!isWhitesTurn && piece.position.y == board.height - 1)) {
                move.eatenPieces.Add(piece);
                board.SendToGraveyard(piece);
                board.setPiece(PieceType.CH_QUEE, isWhitesTurn, move.end); 
                return true;
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

    public override bool CheckForEnd(ref bool? whiteWon, bool isWhitesTurn) {
        if (!board.GetAllMoves(isWhitesTurn).Any()) {
            if (isCheck(!isWhitesTurn))
                whiteWon = !isWhitesTurn;
            else
                whiteWon = null;

            return true;
        }
        return false;
    }

    private int calculatePawnsPenalty (bool isWhitesTurn) {
        int penalty = 0;
        foreach (Piece pawn in board.GetPieces(PieceType.CH_PAWN)) {
            // Blocked pawns penalty
            if (!pawn.PossibleMoves(isWhitesTurn, eliminateMoves: false).Any()) {
                penalty += pawn.isWhite == isWhitesTurn ? 1 : -1;
            }
        }
        return penalty;
    }

    public override int scoreBoard (bool isWhitesTurn) {
        IEnumerable<Move> opponentMoves = board.GetAllMoves(!isWhitesTurn);
        if (!opponentMoves.Any() && isCheck(!isWhitesTurn)) {
            return int.MinValue / 2;
        }

        IEnumerable<Move> moves = board.GetAllMoves(isWhitesTurn);
        if (!moves.Any() && isCheck(isWhitesTurn)) {
            return int.MaxValue / 2;
        }

        int movementScore = 0; // moves.ToList().Count - opponentMoves.ToList().Count;
        int pawnsPenalty = 0; // calculatePawnsPenalty(isWhitesTurn);

        return 100 * materialScore() + 10 * movementScore - 50 * pawnsPenalty;
    }
}
