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

    public static bool operator == (Position position1, Position position2) {
        if ((System.Object)position1 == null || (System.Object)position2 == null) {
            return false;
        }

        return position1.x == position2.x && position1.y == position2.y;
    }

    public static bool operator != (Position position1, Position position2) {
        return !(position1 == position2);
    }

    public bool Equals (Position other) {
        return this == other;
    }

    public override bool Equals (object obj) {
        if (obj == null) {
            return false;
        }

        Position objAsPosition = obj as Position;
        if (objAsPosition == null) {
            return false;
        } else {
            return Equals(objAsPosition);
        }
    }

    public override int GetHashCode () {
        return base.GetHashCode();
    }
}

public class Move {
    public Position start;
    public Position end;

    public Move (Position start, Position end) {
        this.start = start;
        this.end = end;
    }

    public override string ToString () {
        return "start: " + start + "; end " + end;
    }

    public static bool operator == (Move move1, Move move2) {
        if ((System.Object) move1 == null || (System.Object)move2 == null) {
            return false;
        }

        return move1.start == move2.start && move1.end == move2.end;
    }

    public static bool operator != (Move move1, Move move2) {
        return !(move1 == move2);
    }

    public bool Equals (Move other) {
        return this == other;
    }

    public override bool Equals (object obj) {
        if (obj == null) {
            return false;
        }

        Move objAsMove = obj as Move;
        if (objAsMove == null) {
            return false;
        } else {
            return Equals(objAsMove);
        }
    }

    public override int GetHashCode () {
        return base.GetHashCode();
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

    public bool isWhite;

    [HideInInspector]
    public Board board;

    [HideInInspector]
    public Game game;

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

        if (game.isWhitesTurn == isWhite) {
            if (orthoMovement) {
                possibleMoves.AddRange(GetOrthoMoves());
            }
        }

        return possibleMoves;
    }

    public void OnClick () {
        List<Move> possibleMoves = PossibleMoves();
        board.previousMoves = possibleMoves;
        board.MarkFields(position, possibleMoves);
    }
}
