using UnityEngine;
using System.Collections;

public class VikingChess : Game {

    public VikingChess ()
        : base(GameName.VIKING_CHESS, "Viking Chess", "", null) { }

    void SetBoardAndPieces() {
        board = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        Board boardScript = board.GetComponent<Board>();
        boardScript.BuildBoard(9, 9, BoardType.CHECKERED);
        boardScript.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.ROOK, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK },
                                                 { PieceType.ROOK, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.ROOK },
                                                 { PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.ROOK, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              false);
        boardScript.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.ROOK, PieceType.KING, PieceType.ROOK, PieceType.ROOK, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              true);
    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }
}
