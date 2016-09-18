using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {
    public bool orthoMovement;
    public bool singleOrthoMovement;
    public bool chessOrthoMovement;
    public bool chessDiagonalMovement;
    public bool knightMovement;
    public bool pawnChessMovement;
    public bool kingChessMovement;
    public bool pawnCheckersMovement;
    public bool kingCheckersMovement;
    public bool pawnReversiMovement;
    public bool foxMovement;
    public bool houndMovement;

    public PieceType pieceType;
    public bool isWhite;
    public int score;

    [HideInInspector]
    public Board board;

    [HideInInspector]
    public Game game;

    [HideInInspector]
    public Position position;

    private IEnumerable<Move> GetOrthoMoves (Direction direction) {
        Position pos = position + direction;
        for (int i = 1; board.ValidPosition(pos); i++, pos = position + i * direction) {
            if (game.CanMoveTo(pos, pieceType))
                yield return new Move(position, pos);
            else
                break;
        }
    }

    private IEnumerable<Move> GetOrthoMoves() {
        foreach (Move move in GetOrthoMoves(new Direction(1, 0, null)))
            yield return move;
        foreach (Move move in GetOrthoMoves(new Direction(-1, 0, null)))
            yield return move;
        foreach (Move move in GetOrthoMoves(new Direction(0, 1, null)))
            yield return move;
        foreach (Move move in GetOrthoMoves(new Direction(0, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetSingleOrthoMoves(Direction direction) {
        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            yield return new Move(position, pos);
    }

    private IEnumerable<Move> GetSingleOrthoMoves() {
        foreach (Move move in GetSingleOrthoMoves(new Direction(1, 0, null)))
            yield return move;
        foreach (Move move in GetSingleOrthoMoves(new Direction(-1, 0, null)))
            yield return move;
        foreach (Move move in GetSingleOrthoMoves(new Direction(0, 1, null)))
            yield return move;
        foreach (Move move in GetSingleOrthoMoves(new Direction(0, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetChessOrthoMoves(Direction direction) {
        Position pos = position + direction;

        for (int i = 1; board.ValidPosition(pos); i++, pos = position + i * direction) {
            Piece piece = board.GetField(pos).FindPiece();

            if (game.CanMoveTo(pos, pieceType))
                yield return new Move(position, pos);
            else if (piece && piece.isWhite != isWhite) {
                yield return new Move(position, pos);
                break;
            } else
                break;
        }
    }

    private IEnumerable<Move> GetChessOrthoMoves() {
        foreach (Move move in GetChessOrthoMoves(new Direction(1, 0, null)))
            yield return move;
        foreach (Move move in GetChessOrthoMoves(new Direction(-1, 0, null)))
            yield return move;
        foreach (Move move in GetChessOrthoMoves(new Direction(0, 1, null)))
            yield return move;
        foreach (Move move in GetChessOrthoMoves(new Direction(0, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetChessDiagonalMoves(Direction direction) {
        Position pos = position + direction;
        for (int i = 1; board.ValidPosition(pos); i++, pos = position + i * direction) {
            Piece piece = board.GetField(pos).FindPiece();
            if (game.CanMoveTo(pos)) {
                yield return new Move(position, pos);
            } else if (board.ValidPosition(pos) && piece && piece.isWhite != isWhite) {
                yield return new Move(position, pos);
                break;
            } else
                break;
        }
    }

    private IEnumerable<Move> GetChessDiagonalMoves() {
        foreach (Move move in GetChessDiagonalMoves(new Direction(1, 1, null)))
            yield return move;
        foreach (Move move in GetChessDiagonalMoves(new Direction(1, -1, null)))
            yield return move;
        foreach (Move move in GetChessDiagonalMoves(new Direction(-1, 1, null)))
            yield return move;
        foreach (Move move in GetChessDiagonalMoves(new Direction(-1, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetKnightMoves(Direction direction) {
        Position pos = position + direction;

        if (!board.ValidPosition(pos))
            yield break;

        Piece piece = board.GetField(pos).FindPiece();

        if (game.CanMoveTo(pos))
            yield return new Move(position, pos);
        else if (piece && piece.isWhite != isWhite) {
            yield return new Move(position, pos);
        }
    }

    private IEnumerable<Move> GetKnightMoves() {
        foreach (Move move in GetKnightMoves(new Direction(2, 1, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(2, -1, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(-2, 1, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(-2, -1, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(1, 2, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(1, -2, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(-1, 2, null)))
            yield return move;
        foreach (Move move in GetKnightMoves(new Direction(-1, -2, null)))
            yield return move;
    }

    private IEnumerable<Move> GetPawnChessMoves(Direction direction) {
        Position pos = position + direction;

        if (!board.ValidPosition(pos))
            yield break;

        Piece piece = board.GetField(pos).FindPiece();

        if (direction.x == 0) {
            if (game.CanMoveTo(pos))
                yield return new Move(position, pos);
        } else {
            if (piece && piece.isWhite != isWhite)
                yield return new Move(position, pos);
        }
    }

    private IEnumerable<Move> GetPawnChessMoves() {
        if (isWhite) {
            foreach (Move move in GetPawnChessMoves(new Direction(0, -1, null)))
                yield return move;
            foreach (Move move in GetPawnChessMoves(new Direction(-1, -1, null)))
                yield return move;
            foreach (Move move in GetPawnChessMoves(new Direction(1, -1, null)))
                yield return move;
            if (position.y == board.height - 2) {
                foreach (Move move in GetPawnChessMoves(new Direction(0, -2, null)))
                    yield return move;
            }
        } else {
            foreach (Move move in GetPawnChessMoves(new Direction(0, 1, null)))
                yield return move;
            foreach (Move move in GetPawnChessMoves(new Direction(-1, 1, null)))
                yield return move;
            foreach (Move move in GetPawnChessMoves(new Direction(1, 1, null)))
                yield return move;
            if (position.y == 1) {
                foreach (Move move in GetPawnChessMoves(new Direction(0, 2, null)))
                    yield return move;
            }
        }
    }

    private IEnumerable<Move> GetKingChessMoves(Direction direction) {
        Position pos = position + direction;

        if (!board.ValidPosition(pos))
            yield break;

        Piece piece = board.GetField(pos).FindPiece();

        if (game.CanMoveTo(pos))
            yield return new Move(position, pos);
        else if (piece && piece.isWhite != isWhite) {
            yield return new Move(position, pos);
        }
    }

    private IEnumerable<Move> GetKingChessMoves() {
        foreach (Move move in GetKingChessMoves(new Direction(1, 0, null)))
            yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(1, 1, null)))
                yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(1, -1, null)))
            yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(0, 1, null)))
            yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(0, -1, null)))
            yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(-1, 0, null)))
                yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(-1, 1, null)))
                yield return move;
        foreach (Move move in GetKingChessMoves(new Direction(-1, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetPawnCheckersMoves(Direction direction) {
        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            if (Math.Abs(direction.x) == 2) {
                if (!game.CanMoveTo(position + new Position(direction.x / 2, direction.y / 2, null)))
                    if (board.GetField(position + new Position(direction.x / 2, direction.y / 2, null)).FindPiece().isWhite != isWhite)
                        yield return new Move(position, pos, true);
            } else
                yield return new Move(position, pos);
    }

    private IEnumerable<Move> GetPawnCheckersMoves() {
        bool movesReturned = false;
        if (isWhite) {
            foreach (Move move in GetPawnCheckersMoves(new Direction(2, -2, null))) {
                yield return move;
                movesReturned = true;
            }
            foreach (Move move in GetPawnCheckersMoves(new Direction(-2, -2, null))) {
                yield return move;
                movesReturned = true;
            }
        if (!movesReturned) {
                foreach (Move move in GetPawnCheckersMoves(new Direction(1, -1, null)))
                    yield return move;
                foreach (Move move in GetPawnCheckersMoves(new Direction(-1, -1, null)))
                    yield return move;
            }
        } else {
            foreach (Move move in GetPawnCheckersMoves(new Direction(2, 2, null))) {
                yield return move;
                movesReturned = true;
            }
            foreach (Move move in GetPawnCheckersMoves(new Direction(-2, 2, null))) {
                yield return move;
                movesReturned = true;
            }
            if (!movesReturned) {
                foreach (Move move in GetPawnCheckersMoves(new Direction(1, 1, null)))
                    yield return move;
                foreach (Move move in GetPawnCheckersMoves(new Direction(-1, 1, null)))
                    yield return move;
            }
        }
    }

    private IEnumerable<Move> GetKingCheckersMoves(Direction direction) {
        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            if (Math.Abs(direction.x) == 2) {
                if (!game.CanMoveTo(position + new Position(direction.x / 2, direction.y / 2, null)))
                    if (board.GetField(position + new Position(direction.x / 2, direction.y / 2, null)).FindPiece().isWhite != isWhite)
                        yield return new Move(position, pos, true);
            } else
                yield return new Move(position, pos);
    }

    private IEnumerable<Move> GetKingCheckersMoves() {
        bool movesReturned = false;
        foreach (Move move in GetKingCheckersMoves(new Direction(2, -2, null))) {
            yield return move;
            movesReturned = true;
        }
        foreach (Move move in GetKingCheckersMoves(new Direction(-2, -2, null))) {
            yield return move;
            movesReturned = true;
        }
        foreach (Move move in GetKingCheckersMoves(new Direction(2, 2, null))) {
            yield return move;
            movesReturned = true;
        }
        foreach (Move move in GetKingCheckersMoves(new Direction(-2, 2, null))) {
            yield return move;
            movesReturned = true;
        }
        if (!movesReturned) {
            foreach (Move move in GetKingCheckersMoves(new Direction(1, -1, null)))
                yield return move;
            foreach (Move move in GetKingCheckersMoves(new Direction(-1, -1, null)))
                yield return move;
            foreach (Move move in GetKingCheckersMoves(new Direction(1, 1, null)))
                yield return move;
            foreach (Move move in GetKingCheckersMoves(new Direction(-1, 1, null)))
                yield return move;
        }
    }

    private IEnumerable<Move> GetHoundMoves(Direction direction) {
        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            yield return new Move(position, pos);
    }

    private IEnumerable<Move> GetHoundMoves() {
        foreach (Move move in GetHoundMoves(new Direction(1, -1, null)))
            yield return move;
        foreach (Move move in GetHoundMoves(new Direction(-1, -1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetFoxMoves(Direction direction) {
        Position pos = position + direction;

        if (board.ValidPosition(pos) && game.CanMoveTo(pos))
            yield return new Move(position, pos);
    }

    private IEnumerable<Move> GetFoxMoves() {
        foreach (Move move in GetFoxMoves(new Direction(1, -1, null)))
            yield return move;
        foreach (Move move in GetFoxMoves(new Direction(-1, -1, null)))
            yield return move;
        foreach (Move move in GetFoxMoves(new Direction(1, 1, null)))
            yield return move;
        foreach (Move move in GetFoxMoves(new Direction(-1, 1, null)))
            yield return move;
    }

    private IEnumerable<Move> GetPawnReversiMoves (Position pos, Direction direction) {
        Position currentPosition = pos + direction;

        if(!board.ValidPosition(currentPosition))
            yield break;

        Piece piece = board.GetField(currentPosition).FindPiece();

        if (piece && piece.isWhite != game.isWhitesTurn) {
            ((Reversi)game).isAttack = true;
            foreach (Move move in GetPawnReversiMoves(currentPosition, direction))
                yield return move;
        } else if (((Reversi)game).isAttack && game.CanMoveTo(currentPosition)) {
            yield return new Move(position, currentPosition, true);
        }
    }

    public IEnumerable<Move> GetPawnReversiMoves() {
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(1, 0, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(1, 1, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(1, -1, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(0, 1, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(0, -1, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(-1, 0, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(-1, 1, null)))
            yield return move;
        ((Reversi)game).isAttack = false;
        foreach (Move move in GetPawnReversiMoves(position, new Direction(-1, -1, null)))
            yield return move;
    }

    public IEnumerable<Move> PossibleMoves (bool isWhitesTurn, bool eliminateMoves = true) {
        if (isWhitesTurn == isWhite) {
            if (orthoMovement) {
                foreach (Move move in GetOrthoMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (singleOrthoMovement) {
                foreach (Move move in GetChessDiagonalMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (chessOrthoMovement) {
                foreach (Move move in GetChessOrthoMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (chessDiagonalMovement) {
                foreach (Move move in GetChessDiagonalMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (knightMovement) {
                foreach (Move move in GetKnightMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (pawnChessMovement) {
                foreach (Move move in GetPawnChessMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (kingChessMovement) {
                foreach (Move move in GetKingChessMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (pawnCheckersMovement) {
                foreach (Move move in GetPawnCheckersMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (kingCheckersMovement) {
                foreach (Move move in GetKingCheckersMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (foxMovement) {
                foreach (Move move in GetFoxMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (houndMovement) {
                foreach (Move move in GetHoundMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
            if (pawnReversiMovement) {
                foreach (Move move in ((Reversi)game).returnPossibleMoves())
                    if (!eliminateMoves || game.isMovePossible(move, isWhite))
                        yield return move;
            }
        }
    }

    public void OnClick () {
        List<Move> possibleMoves = PossibleMoves(game.isWhitesTurn).ToList();
        board.previousPossibleMoves = possibleMoves;
        board.game.MarkFields(position, possibleMoves);
    }
}
