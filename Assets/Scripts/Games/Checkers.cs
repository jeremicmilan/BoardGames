using UnityEngine;
using System.Collections;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces() {
        board = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        Board boardScript = board.GetComponent<Board>();
        boardScript.BuildBoard(8, 8, BoardType.CHECKERED);

        boardScript.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK },
                                                 { PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.ROOK },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              true);
        boardScript.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
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
