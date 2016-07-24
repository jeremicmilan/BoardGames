using UnityEngine;
using System.Collections;

public class VikingChess : Game {

    public VikingChess ()
        : base(GameName.VIKING_CHESS, "Viking Chess", "", null) { }

    public override void StartSinglePlayer () {
        board = Object.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        Board boardScript = board.GetComponent<Board>();
        boardScript.BuildBoard(15, 9, BoardType.CHECKERED);
    }

    public override void StartTwoPlayer () {

    }
}
