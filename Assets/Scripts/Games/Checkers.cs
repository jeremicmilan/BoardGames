using UnityEngine;
using System.Collections;
using System;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;

        board.BuildBoard(8, 8, BoardType.CHECKERED);


        board.SetPieces(new PieceType[,] { { PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE,     PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN,   PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE,     PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN,   PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE,     PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN,   PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE,     PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN,   PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.C_PAWN,  PieceType.NONE,     PieceType.C_PAWN },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE,    PieceType.C_PAWN,   PieceType.NONE }},
                              true);

    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }

    public override bool Attack (Move move, bool destroy = true) {
        if (Math.Abs(move.end.x - move.start.x) == 2) {
            if (destroy)
                GameObject.Destroy(board.GetField(move.start.x + (move.end.x - move.start.x) / 2, move.start.y + (move.end.y - move.start.y) / 2).FindPiece().gameObject);
            return true;
        }
       

        return false;
    }
}
