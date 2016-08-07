using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Game {

    public GameName gameName;
    public string name;
    public string description;
    public GameObject picture;

    public Board board;

    public bool isWhitesTurn;
    public bool gameEnded = false;

    public Game (GameName gameName, string name, string description, GameObject picture) {
        this.gameName = gameName;
        this.name = name;
        this.description = name;
        this.picture = picture;
    }

    public abstract void StartSinglePlayer ();
    public abstract void StartTwoPlayer ();

    public abstract bool Attack (Move move, bool destroy = true);
    public abstract void MakeMove(Move move);
    public abstract void MarkFields(Position start, List<Move> possibleMoves);

    public abstract bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE);

    public bool CanMoveTo (Position position, PieceType pieceType = PieceType.AL_NONE) {
        return CanMoveTo(position.x, position.y, pieceType);
    }
    public abstract bool CheckForEnd (ref bool? whiteWon);
    public abstract bool CheckForPieceEvolve (Move move);
}
