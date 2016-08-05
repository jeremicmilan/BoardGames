﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VikingChess : Game {

    List<Position> visitedPositions = new List<Position>();

    public VikingChess ()
        : base(GameName.VIKING_CHESS, "Viking Chess", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(9, 9, BoardType.CUSTOM,
                         new FieldType[,] { { FieldType.ESCAP, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK, FieldType.BLACK, FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.ESCAP },
                                            { FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR },
                                            { FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.WHITE, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR },
                                            { FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.WHITE, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK },
                                            { FieldType.BLACK, FieldType.BLACK, FieldType.WHITE, FieldType.WHITE, FieldType.CASTL, FieldType.WHITE, FieldType.WHITE, FieldType.BLACK, FieldType.BLACK },
                                            { FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.WHITE, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK },
                                            { FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.WHITE, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR },
                                            { FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR, FieldType.NEUTR },
                                            { FieldType.ESCAP, FieldType.NEUTR, FieldType.NEUTR, FieldType.BLACK, FieldType.BLACK, FieldType.BLACK, FieldType.NEUTR, FieldType.NEUTR, FieldType.ESCAP } });

        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK },
                                           { PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.VK_ROOK },
                                           { PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                        false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.VK_KING, PieceType.VK_ROOK, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                        true);
    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }

    private bool Attack (Move move, Position direction, bool destroy = false) {
        Position position = move.end;
        Piece opponentPiece = board.ValidPosition(position + direction) ? board.GetPiece(position + direction) : null;
        if (opponentPiece && opponentPiece.isWhite != isWhitesTurn && opponentPiece.pieceType != PieceType.VK_KING) {
            Position allyPosition = position + 2 * direction;
            if (board.ValidPosition(allyPosition)) {
                Piece piece = board.GetPiece(allyPosition);
                if (piece && piece.isWhite == isWhitesTurn) {
                    if (destroy) {
                        move.eatenPieces.Add(opponentPiece);
                        board.SendToGraveyard(opponentPiece);
                    }
                    return true;
                }
            }
        }
        
        return false;
    }

    public override bool Attack (Move move, bool destroy = true) {
        Piece piece = move.end.field.FindPiece();
        piece = piece ? piece : move.start.field.FindPiece();

        bool attacked = false;

        if (piece.pieceType == PieceType.VK_ROOK) {
            if (Attack(move, new Position(1, 0, null), destroy)) {
                attacked = true;
            }
            if (Attack(move, new Position(-1, 0, null), destroy)) {
                attacked = true;
            }
            if (Attack(move, new Position(0, 1, null), destroy)) {
                attacked = true;
            }
            if (Attack(move, new Position(0, -1, null), destroy)) {
                attacked = true;
            }
        }

        return attacked;
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        FieldType fieldType = field.fieldType;
        Piece piece = field.FindPiece();
        return (!piece && fieldType != FieldType.CASTL && fieldType != FieldType.ESCAP) ||
               (!piece && pieceType == PieceType.VK_KING);
    }

    private bool DidKingEscape () {
        Piece piece = board.GetField(0, 0).FindPiece();
        if (piece && piece.pieceType == PieceType.VK_KING) {
            return true;
        }
        piece = board.GetField(board.width - 1, 0).FindPiece();
        if (piece && piece.pieceType == PieceType.VK_KING) {
            return true;
        }
        piece = board.GetField(0, board.height - 1).FindPiece();
        if (piece && piece.pieceType == PieceType.VK_KING) {
            return true;
        }
        piece = board.GetField(board.width - 1, board.height - 1).FindPiece();
        if (piece && piece.pieceType == PieceType.VK_KING) {
            return true;
        }

        return false;
    }

    private bool IsSurrounded (Position position) {
        if (visitedPositions.Contains(position)) {
            return true;
        }
        visitedPositions.Add(position);

        Piece piece = board.GetField(position).FindPiece();

        if (!board.ValidPosition(position)) {
            return true;
        }

        if (!piece) {
            return false;
        } else if (piece.isWhite == false) {
            return true;
        } else {
            return IsSurrounded(position + new Position(1, 0, null)) &&
                   IsSurrounded(position + new Position(0, 1, null)) &&
                   IsSurrounded(position + new Position(-1, 0, null)) &&
                   IsSurrounded(position + new Position(0, -1, null));
        }
    }

    private bool IsKingCaptured () {
        visitedPositions.Clear();

        return IsSurrounded(board.FindPiece(PieceType.VK_KING).position);
    }

    public override bool CheckForEnd (ref bool whiteWon) {
        if (DidKingEscape()) {
            whiteWon = true;
            return true;
        }
        if (IsKingCaptured()) {
            whiteWon = false;
            return true;
        }
        return false;
    }

    public override bool CheckForPieceEvolve (Move move) {
        return false;
    }
}
