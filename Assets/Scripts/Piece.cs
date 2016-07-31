﻿using UnityEngine;
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

    public static Position operator + (Position position1, Position position2) {
        if ((System.Object)position1 == null) {
            return position2;
        }
        if ((System.Object)position2 == null) {
            return position1;
        }

        Board board = GameObject.FindGameObjectWithTag("board").GetComponent<Board>();
        int x = position1.x + position2.x;
        int y = position1.y + position2.y;

        return new Position(x, y, board.ValidPosition(x, y) ? board.GetField(position1.x + position2.x, position1.y + position2.y) : null);
    }

    public static Position operator * (int k, Position position) {
        if ((System.Object)position == null) {
            return null;
        }

        return new Position(k * position.x, k * position.y, position.field);
    }

    public static Position operator * (Position position, int k) {
        return k * position;
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
    public bool kingCheckersMovement;

    public PieceType pieceType;
    public bool isWhite;

    [HideInInspector]
    public Board board;

    [HideInInspector]
    public Game game;

    [HideInInspector]
    public Position position;

    private List<Move> GetOrthoMoves (Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;
        for (int i = 1; board.ValidPosition(pos); i++, pos = position + i * direction) {
            if (!board.IsOcupied(pos)) {
                possibleMoves.Add(new Move(position, pos));
            } else if (!board.CanJumpOver(pos)) {
                break;
            }
        }

        return possibleMoves;
    }

    private List<Move> GetOrthoMoves () {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetOrthoMoves(new Position(1, 0, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(-1, 0, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(0, 1, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(0, -1, null)));

        return possibleMoves;
    }

    private List<Move> GetSingleOrthoMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (position.x + 1 < board.width && !board.IsOcupied(position.x + 1, position.y))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y, board.GetField(position.x + 1, position.y))));

        if (position.x - 1 >= 0 && !board.IsOcupied(position.x - 1, position.y))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y, board.GetField(position.x - 1, position.y))));

        if (position.y + 1 < board.height && !board.IsOcupied(position.x, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y + 1, board.GetField(position.x, position.y + 1))));

        if (position.y - 1 >= 0 && !board.IsOcupied(position.x, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y - 1, board.GetField(position.x, position.y - 1))));

        return possibleMoves;
    }

    private List<Move> GetDiagonalMoves() {
        List<Move> possibleMoves = new List<Move>();

        for (int i = position.x + 1, j = position.y + 1; i < board.width && j < board.height; i++, j++) {
            if (!board.IsOcupied(i, j)) {
                possibleMoves.Add(new Move(position, new Position(i, j, board.GetField(i, j))));
            } else if (!board.CanJumpOver(i, j)) {
                break;
            }
        }

        for (int i = position.x + 1, j = position.y - 1; i < board.width && j >= 0; i++, j--) {
            if (!board.IsOcupied(i, j)) {
                possibleMoves.Add(new Move(position, new Position(i, j, board.GetField(i, j))));
            } else if (!board.CanJumpOver(i, j)) {
                break;
            }
        }

        for (int i = position.x - 1, j = position.y + 1; i >= 0 && j < board.height; i--, j++) {
            if (!board.IsOcupied(i, j)) {
                possibleMoves.Add(new Move(position, new Position(i, j, board.GetField(i, j))));
            } else if (!board.CanJumpOver(i, j)) {
                break;
            }
        }

        for (int i = position.x - 1, j = position.y - 1; i >= 0 && j >= 0; i--, j--) {
            if (!board.IsOcupied(i, j)) {
                possibleMoves.Add(new Move(position, new Position(i, j, board.GetField(i, j))));
            } else if (!board.CanJumpOver(i, j)) {
                break;
            }
        }

        return possibleMoves;
    }

    private List<Move> GetKnightMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (board.ValidPosition(position.x + 2, position.y + 1) && !board.IsOcupied(position.x + 2, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 2, position.y + 1, board.GetField(position.x + 2, position.y + 1))));

        if (board.ValidPosition(position.x + 2, position.y - 1) && !board.IsOcupied(position.x + 2, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 2, position.y - 1, board.GetField(position.x + 2, position.y - 1))));

        if (board.ValidPosition(position.x - 2, position.y + 1) && !board.IsOcupied(position.x - 2, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 2, position.y + 1, board.GetField(position.x - 2, position.y + 1))));

        if (board.ValidPosition(position.x - 2, position.y - 1) && !board.IsOcupied(position.x - 2, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 2, position.y - 1, board.GetField(position.x - 2, position.y - 1))));

        if (board.ValidPosition(position.x + 1, position.y + 2) && !board.IsOcupied(position.x + 1, position.y + 2))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y + 2, board.GetField(position.x + 1, position.y + 2))));

        if (board.ValidPosition(position.x + 1, position.y - 2) && !board.IsOcupied(position.x + 1, position.y - 2))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y - 2, board.GetField(position.x + 1, position.y - 2))));

        if (board.ValidPosition(position.x - 1, position.y + 2) && !board.IsOcupied(position.x - 1, position.y + 2))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y + 2, board.GetField(position.x - 1, position.y + 2))));

        if (board.ValidPosition(position.x - 1, position.y - 2) && !board.IsOcupied(position.x - 1, position.y - 2))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y - 2, board.GetField(position.x - 1, position.y - 2))));

        return possibleMoves;
    }

    private List<Move> GetPawnChessMoves() {
        List<Move> possibleMoves = new List<Move>();

        int orientation;

        if (isWhite)
            orientation = -1;
        else
            orientation = 1;

        if (board.ValidPosition(position.x, position.y + orientation) && !board.IsOcupied(position.x, position.y + orientation))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y + orientation, board.GetField(position.x, position.y + orientation))));
      
        if (position.y == 6 && isWhite && !board.IsOcupied(position.x, position.y - 2))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y - 2, board.GetField(position.x, position.y - 2))));

        if (position.y == 1 && !isWhite && !board.IsOcupied(position.x, position.y + 2))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y + 2, board.GetField(position.x, position.y + 2))));

        return possibleMoves;
    }

    private List<Move> GetKingChessMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (board.ValidPosition(position.x + 1, position.y + 1) && !board.IsOcupied(position.x + 1, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y + 1, board.GetField(position.x + 1, position.y + 1))));

        if (board.ValidPosition(position.x + 1, position.y - 1) && !board.IsOcupied(position.x + 1, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y - 1, board.GetField(position.x + 1, position.y - 1))));

        if (board.ValidPosition(position.x - 1, position.y + 1) && !board.IsOcupied(position.x - 1, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y + 1, board.GetField(position.x - 1, position.y + 1))));

        if (board.ValidPosition(position.x - 1, position.y - 1) && !board.IsOcupied(position.x - 1, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y - 1, board.GetField(position.x - 1, position.y - 1))));

        if (board.ValidPosition(position.x, position.y + 1) && !board.IsOcupied(position.x, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y + 1, board.GetField(position.x, position.y + 1))));

        if (board.ValidPosition(position.x, position.y - 1) && !board.IsOcupied(position.x, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x, position.y - 1, board.GetField(position.x, position.y - 1))));

        if (board.ValidPosition(position.x + 1, position.y) && !board.IsOcupied(position.x + 1, position.y))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y, board.GetField(position.x + 1, position.y))));

        if (board.ValidPosition(position.x - 1, position.y) && !board.IsOcupied(position.x - 1, position.y))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y, board.GetField(position.x - 1, position.y))));


        return possibleMoves;
    }

    private List<Move> GetPawnCheckersMoves() {
        List<Move> possibleMoves = new List<Move>();

        int orientation;

        if (isWhite)
            orientation = -1;
        else
            orientation = 1;

        if (board.ValidPosition(position.x +  1, position.y + orientation) && !board.IsOcupied(position.x + 1, position.y + orientation))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y + orientation, board.GetField(position.x + 1, position.y + orientation))));

        if (board.ValidPosition(position.x - 1, position.y + orientation) && !board.IsOcupied(position.x - 1, position.y + orientation))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y + orientation, board.GetField(position.x - 1, position.y + orientation))));

        return possibleMoves;
    }

    private List<Move> GetKingCheckersMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (board.ValidPosition(position.x + 1, position.y + 1) && !board.IsOcupied(position.x + 1, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y + 1, board.GetField(position.x + 1, position.y + 1))));

        if (board.ValidPosition(position.x - 1, position.y + 1) && !board.IsOcupied(position.x - 1, position.y + 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y + 1, board.GetField(position.x - 1, position.y + 1))));

        if (board.ValidPosition(position.x + 1, position.y - 1) && !board.IsOcupied(position.x + 1, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x + 1, position.y - 1, board.GetField(position.x + 1, position.y - 1))));

        if (board.ValidPosition(position.x - 1, position.y - 1) && !board.IsOcupied(position.x - 1, position.y - 1))
            possibleMoves.Add(new Move(position, new Position(position.x - 1, position.y - 1, board.GetField(position.x - 1, position.y - 1))));

        return possibleMoves;
    }

    private List<Move> PossibleMoves () {
        List<Move> possibleMoves = new List<Move>();

        if (game.isWhitesTurn == isWhite) {
            if (orthoMovement) {
                possibleMoves.AddRange(GetOrthoMoves());
            }
            if (singleOrthoMovement) {
                possibleMoves.AddRange(GetDiagonalMoves());
            }
            if (diagonalMovement) {
                possibleMoves.AddRange(GetDiagonalMoves());
            }
            if (knightMovement) {
                possibleMoves.AddRange(GetKnightMoves());
            }
            if (pawnChessMovement) {
                possibleMoves.AddRange(GetPawnChessMoves());
            }
            if (kingChessMovement) {
                possibleMoves.AddRange(GetKingChessMoves());
            }
            if (pawnCheckersMovement) {
                possibleMoves.AddRange(GetPawnCheckersMoves());
            }
            if (kingCheckersMovement) {
                possibleMoves.AddRange(GetKingCheckersMoves());
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
