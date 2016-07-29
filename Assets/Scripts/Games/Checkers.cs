using UnityEngine;
using System.Collections;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces() {
        GameObject boardObject = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

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
}
