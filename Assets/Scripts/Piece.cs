using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Position {
    public int x;
    public int y;
    public Field field;

    public Position (int x, int y, Field field) {
        this.x = x;
        this.y = y;
        this.field = field;
    }

    public override string ToString () {
        return "(" + x + ", " + y + ")";
    }
}

public class Move {
    public Position start;
    public Position end;
    public GameObject field;

    public Move (Position start, Position end) {
        this.start = start;
        this.end = end;
    }

    public override string ToString () {
        return "start: " + start + "; end " + end;
    }
}

public class Piece : MonoBehaviour {

    public bool orthoMovement;
    public bool singleOrthoMovement;
    public bool diagonalMovement;
    public bool knightMovement;
    public bool pawnChessMovement;
    public bool kingChessMovement;
    public bool pawnCheckersMovement;

    [HideInInspector]
    public Board board;

    [HideInInspector]
    public Position position;

    private List<Move> GetOrthoMoves () {
        List<Move> possibleMoves = new List<Move>();

        for (int i = position.x + 1; i < board.width; i++) {
            if (!board.IsOcupied(i, position.y)) {
                possibleMoves.Add(new Move(position, new Position(i, position.y, board.GetField(i, position.y))));
            } else if (!board.CanJumpOver(i, position.y)) {
                break;
            }
        }

        for (int i = position.x - 1; i >= 0; i--) {
            if (!board.IsOcupied(i, position.y)) {
                possibleMoves.Add(new Move(position, new Position(i, position.y, board.GetField(i, position.y))));
            } else if (!board.CanJumpOver(i, position.y)) {
                break;
            }
        }

        for (int j = position.y + 1; j < board.height; j++) {
            if (!board.IsOcupied(position.x, j)) {
                possibleMoves.Add(new Move(position, new Position(position.x, j, board.GetField(position.x, j))));
            } else if (!board.CanJumpOver(position.x, j)) {
                break;
            }
        }

        for (int j = position.y - 1; j >= 0; j--) {
            if (!board.IsOcupied(position.x, j)) {
                possibleMoves.Add(new Move(position, new Position(position.x, j, board.GetField(position.x, j))));
            } else if (!board.CanJumpOver(position.x, j)) {
                break;
            }
        }

        return possibleMoves;
    }

    private List<Move> PossibleMoves () {
        List<Move> possibleMoves = new List<Move>();

        if (orthoMovement) {
            possibleMoves.AddRange(GetOrthoMoves());
        }

        return possibleMoves;
    }

    public void OnClick () {
        List<Move> possibleMoves = PossibleMoves();

        board.MarkFields(position, possibleMoves);

        foreach (Move move in possibleMoves) {
            Debug.Log(move);
        }
    }
}
