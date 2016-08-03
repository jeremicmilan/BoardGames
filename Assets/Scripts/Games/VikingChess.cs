using UnityEngine;
using System.Collections;
using System;

public class VikingChess : Game {

    public VikingChess ()
        : base(GameName.VIKING_CHESS, "Viking Chess", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;

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

        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.VK_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK },
                                           { PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.CH_ROOK },
                                           { PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                        false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.VK_KING, PieceType.CH_ROOK, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CH_ROOK, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
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

    private bool Attack (Position position, Position direction, bool destroy = false) {
        Piece opponentPiece = board.ValidPosition(position + direction) ? board.GetPiece(position + direction) : null;
        if (opponentPiece && opponentPiece.isWhite != isWhitesTurn && board.ValidPosition(position + direction + direction)) {
            Piece piece = board.GetPiece(position + direction + direction);
            if (piece && piece.isWhite == isWhitesTurn) {
                if (destroy) {
                    GameObject.Destroy(opponentPiece.gameObject);
                }
                return true;
            }
        }
        return false;
    }

    public override bool Attack (Move move, bool destroy = true) {
        Piece piece = move.end.field.FindPiece();
        piece = piece ? piece : move.start.field.FindPiece();

        bool attacked = false;

        if (piece.pieceType == PieceType.CH_ROOK) {
            if (Attack(move.end, new Position(1, 0, null), destroy)) {
                attacked = true;
            }
            if (Attack(move.end, new Position(-1, 0, null), destroy)) {
                attacked = true;
            }
            if (Attack(move.end, new Position(0, 1, null), destroy)) {
                attacked = true;
            }
            if (Attack(move.end, new Position(0, -1, null), destroy)) {
                attacked = true;
            }
        }

        return attacked;
    }
}
