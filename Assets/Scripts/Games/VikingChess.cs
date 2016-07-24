using UnityEngine;
using System.Collections;

public class VikingChess : Game {

    public VikingChess ()
        : base(GameName.VIKING_CHESS, "Viking Chess", "", null) { }

    public override void StartSinglePlayer () {
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
                              true);
        boardScript.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.ROOK, PieceType.KING, PieceType.ROOK, PieceType.ROOK, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                                 { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              false);
    }

    public override void StartTwoPlayer () {

    }
}
