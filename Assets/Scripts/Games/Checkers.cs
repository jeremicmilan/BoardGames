using UnityEngine;
using System.Collections;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces() {
        GameObject boardObject = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.BuildBoard(8, 8, BoardType.CHECKERED);

        board.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK },
                                           { PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE },
                                           { PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              true);
        board.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE },
                                           { PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK },
                                           { PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE }},
                              false);
    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }
}
