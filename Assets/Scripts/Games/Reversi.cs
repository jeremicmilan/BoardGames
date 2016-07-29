using UnityEngine;
using System.Collections;

public class Reversi : Game {

    public Reversi ()
        : base(GameName.REVERSI, "Reversi", "", null) { }

    void SetBoardAndPieces() {
        GameObject boardObject = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;

        board.BuildBoard(8, 8, BoardType.UNCHECKERED);

        board.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.ROOK, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE },
                                           { PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE, PieceType.NONE }},
                              true);
    }
    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }
}
