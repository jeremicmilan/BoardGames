using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

        return new Position(x, y, board.ValidPosition(x, y) ? board.GetField(x, y) : null);
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

    public List<Piece> eatenPieces = new List<Piece>();

    public bool isAttack = false;

    public Move (Position start, Position end) {
        this.start = start;
        this.end = end;
    }

    public Move(Position start, Position end, bool isAttack) {
        this.start = start;
        this.end = end;
        this.isAttack = isAttack;
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
    public bool pawnReversiMovement;

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
            if (game.CanMoveTo(pos, pieceType)) {
                possibleMoves.Add(new Move(position, pos));
            } else if (!board.CanJumpOver(pos)) {
                break;
            }
        }

        return possibleMoves;
    }

    private List<Move> GetOrthoMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetOrthoMoves(new Position(1, 0, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(-1, 0, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(0, 1, null)));
        possibleMoves.AddRange(GetOrthoMoves(new Position(0, -1, null)));

        return possibleMoves;
    }

    private List<Move> GetSingleOrthoMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;
        
        if (board.ValidPosition(pos) && game.CanMoveTo(pos)) 
            possibleMoves.Add(new Move(position, pos));
               
        return possibleMoves;
    
    }


    private List<Move> GetSingleOrthoMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetSingleOrthoMoves(new Position(1, 0, null)));
        possibleMoves.AddRange(GetSingleOrthoMoves(new Position(-1, 0, null)));
        possibleMoves.AddRange(GetSingleOrthoMoves(new Position(0, 1, null)));
        possibleMoves.AddRange(GetSingleOrthoMoves(new Position(0, -1, null)));

        return possibleMoves;
    }

    private List<Move> GetDiagonalMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;
        for (int i = 1; board.ValidPosition(pos); i++, pos = position + i * direction) {
            if (game.CanMoveTo(pos)) {
                possibleMoves.Add(new Move(position, pos));
            }
        }

        return possibleMoves;
    }

    private List<Move> GetDiagonalMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetDiagonalMoves(new Position(1, 1, null)));
        possibleMoves.AddRange(GetDiagonalMoves(new Position(1, -1, null)));
        possibleMoves.AddRange(GetDiagonalMoves(new Position(-1, 1, null)));
        possibleMoves.AddRange(GetDiagonalMoves(new Position(-1, -1, null)));

        return possibleMoves;
    }

    private List<Move> GetKnightMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            possibleMoves.Add(new Move(position, pos));

        return possibleMoves;
    }

    private List<Move> GetKnightMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetKnightMoves(new Position(2, 1, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(2, -1, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(-2, 1, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(-2, -1, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(1, 2, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(1, -2, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(-1, 2, null)));
        possibleMoves.AddRange(GetKnightMoves(new Position(-1, -2, null)));

       
        return possibleMoves;
    }

    private List<Move> GetPawnChessMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            possibleMoves.Add(new Move(position, pos));

        return possibleMoves;
    }

    private List<Move> GetPawnChessMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (isWhite) {
            possibleMoves.AddRange(GetPawnChessMoves(new Position(0, -1, null)));
            if(position.y == board.height - 2 )
                possibleMoves.AddRange(GetPawnChessMoves(new Position(0, -2, null)));
        } else {
            possibleMoves.AddRange(GetPawnChessMoves(new Position(0, 1, null)));
            if (position.y == 1)
                possibleMoves.AddRange(GetPawnChessMoves(new Position(0, 2, null)));
        }
        
        return possibleMoves;
    }

    private List<Move> GetKingChessMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            possibleMoves.Add(new Move(position, pos));

        return possibleMoves;
    }

    private List<Move> GetKingChessMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetKingChessMoves(new Position(1, 0, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(1, 1, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(1, -1, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(0, 1, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(0, -1, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(-1, 0, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(-1, 1, null)));
        possibleMoves.AddRange(GetKingChessMoves(new Position(-1, -1, null)));


        return possibleMoves;
    }

    private List<Move> GetPawnCheckersMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;
        
        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            if (Math.Abs(direction.x) == 2) {
                if (!game.CanMoveTo(position + new Position(direction.x / 2, direction.y / 2, null)))
                    if (board.GetField(position + new Position(direction.x / 2, direction.y / 2, null)).FindPiece().isWhite != isWhite)
                        possibleMoves.Add(new Move(position, pos, true));
            } else
                possibleMoves.Add(new Move(position, pos));

        return possibleMoves;
    }

    private List<Move> GetPawnCheckersMoves() {
        List<Move> possibleMoves = new List<Move>();

        if (isWhite) {
            possibleMoves.AddRange(GetPawnCheckersMoves(new Position(2, -2, null)));
            possibleMoves.AddRange(GetPawnCheckersMoves(new Position(-2, -2, null)));
            if (possibleMoves.Count == 0) {
                possibleMoves.AddRange(GetPawnCheckersMoves(new Position(1, -1, null)));
                possibleMoves.AddRange(GetPawnCheckersMoves(new Position(-1, -1, null)));
            }
        } else {
            possibleMoves.AddRange(GetPawnCheckersMoves(new Position(2, 2, null)));
            possibleMoves.AddRange(GetPawnCheckersMoves(new Position(-2, 2, null)));
            if (possibleMoves.Count == 0) {
                possibleMoves.AddRange(GetPawnCheckersMoves(new Position(1, 1, null)));
                possibleMoves.AddRange(GetPawnCheckersMoves(new Position(-1, 1, null)));
            }
        }

        return possibleMoves;
    }

    private List<Move> GetKingCheckersMoves(Position direction) {
        List<Move> possibleMoves = new List<Move>();

        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            if (Math.Abs(direction.x) == 2) {
                if (!game.CanMoveTo(position + new Position(direction.x / 2, direction.y / 2, null)))
                    if (board.GetField(position + new Position(direction.x / 2, direction.y / 2, null)).FindPiece().isWhite != isWhite)
                        possibleMoves.Add(new Move(position, pos, true));
            } else
                possibleMoves.Add(new Move(position, pos));

        return possibleMoves;
    }

    private List<Move> GetKingCheckersMoves() {
        List<Move> possibleMoves = new List<Move>();

        possibleMoves.AddRange(GetKingCheckersMoves(new Position(2, -2, null)));
        possibleMoves.AddRange(GetKingCheckersMoves(new Position(-2, -2, null)));
        possibleMoves.AddRange(GetKingCheckersMoves(new Position(2, 2, null)));
        possibleMoves.AddRange(GetKingCheckersMoves(new Position(-2, 2, null)));
        if (possibleMoves.Count == 0) {
            possibleMoves.AddRange(GetKingCheckersMoves(new Position(1, -1, null)));
            possibleMoves.AddRange(GetKingCheckersMoves(new Position(-1, -1, null)));
            possibleMoves.AddRange(GetKingCheckersMoves(new Position(1, 1, null)));
            possibleMoves.AddRange(GetKingCheckersMoves(new Position(-1, 1, null)));
        }

        return possibleMoves;
    }

    private List<Move> GetPawnReversiMoves(Position direction, Position pos) {
        List<Move> possibleMoves = new List<Move>();

        Position currentPosition = pos + direction;

        if(!board.ValidPosition(currentPosition))
            return possibleMoves;

        Piece piece = board.GetField(currentPosition).FindPiece();

        if (piece && piece.isWhite != game.isWhitesTurn) {
            ((Reversi)game).isAttack = true;
            possibleMoves.AddRange(GetPawnReversiMoves(direction, currentPosition));
        } else if (((Reversi)game).isAttack && game.CanMoveTo(currentPosition)) {
            possibleMoves.Add(new Move(position, currentPosition, true));
        }

        return possibleMoves;
    }

    public List<Move> GetPawnReversiMoves() {
        List<Move> possibleMoves = new List<Move>();

        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(1, 0, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(1, 1, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(1, -1, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(0, 1, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(0, -1, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(-1, 0, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(-1, 1, null), position));
        ((Reversi)game).isAttack = false;
        possibleMoves.AddRange(GetPawnReversiMoves(new Position(-1, -1, null), position));

        return possibleMoves;
    }

    public List<Move> PossibleMoves () {
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
            if (pawnReversiMovement) {
                possibleMoves.AddRange(((Reversi)game).returnPossibleMoves());
            }
        }

        return possibleMoves;
    }

    public void OnClick () {
        List<Move> possibleMoves = PossibleMoves();
        board.previousPossibleMoves = possibleMoves;
        board.game.MarkFields(position, possibleMoves);
    }
}
