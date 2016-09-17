using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;

public abstract class Game {

    public GameName gameName;
    public string name;
    public string description;
    public GameObject picture;
    public AI ai;
    public bool playingAgainstAI;

    public Board board;

    public bool isWhitesTurn;
    public bool gameEnded = false;

    public Game (GameName gameName, string name, string description, GameObject picture) {
        this.gameName = gameName;
        this.name = name;
        this.description = name;
        this.picture = picture;
        ai = new AI(this);
    }

    protected abstract void SetBoardAndPieces ();
    protected virtual void GameSpecificStartSinglePlayer () { }
    protected virtual void GameSpecificStartTwoPlayer () { }

    private void StartGame () {
        SetBoardAndPieces();
        board.UpdatePlayerStatusText();
        GameObject.FindGameObjectWithTag("game status").GetComponent<Text>().text = "";
    }

    public void StartSinglePlayer () {
        StartGame();
        playingAgainstAI = true;
        GameSpecificStartSinglePlayer();
    }

    public void StartTwoPlayer () {
        StartGame();
        playingAgainstAI = false;
        GameSpecificStartTwoPlayer();
    }

    public abstract bool Attack (Move move, bool destroy = true);
    public abstract void MarkFields (Position start, List<Move> possibleMoves);

    public virtual void GameSpecificPreMakeMove (ref Move move, Piece piece, bool attacked, bool fake) { }
    public virtual void GameSpecificPostMakeMove (ref Move move, Piece piece, bool attacked, bool fake) { }

    public virtual void MakeMove (Move move, bool fake = false) {
        Assert.IsNotNull(move);
        Piece piece = move.start.field.FindPiece();
        Assert.IsNotNull(piece);

        bool attacked = Attack(move);

        piece.transform.parent = move.end.field.transform;
        piece.position = move.end;
        piece.transform.localPosition = new Vector3(0, 0, -1);

        GameSpecificPreMakeMove(ref move, piece, attacked, fake);

        board.moveHistory.Push(move);
        isWhitesTurn = !isWhitesTurn;

        GameSpecificPostMakeMove(ref move, piece, attacked, fake);

        if (!fake) {
            board.UpdatePlayerStatusText();
            board.ClearMarkers();
        }

        board.previousPossibleMoves.Clear();

        if (playingAgainstAI && !fake && !isWhitesTurn) { // TODO: isWhitesTurn -> isAIsTurn, pick color for single player
            MakeMove(getAIMove());
        }
    }

    public virtual void UndoMove(Move move, bool fake = false) {
        Piece piece = move.end.field.FindPiece();

        piece.transform.parent = move.start.field.transform;
        piece.position = move.start;
        piece.transform.localPosition = new Vector3(0, 0, -1);

        if (!fake) {
            board.ClearMarkers();
        }

        isWhitesTurn = !isWhitesTurn;

        gameEnded = false;
        board.UpdateGameStatusText("");

        foreach (Piece p in move.eatenPieces) {
            p.transform.parent = p.position.field.transform;
            p.transform.localPosition = new Vector3(0, 0, -1);
            p.gameObject.SetActive(true);
        }

        if (!fake) {
            board.UpdatePlayerStatusText();
        }

        if (playingAgainstAI && !fake && !isWhitesTurn) { // TODO: isWhitesTurn -> isAIsTurn, pick color for single player
            board.Undo();
        }
    }

    public virtual List<Move> EliminateImpossibleMoves (List<Move> moves, bool isWhite) {
        return moves;
    }

    public abstract bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE);

    public bool CanMoveTo (Position position, PieceType pieceType = PieceType.AL_NONE) {
        return CanMoveTo(position.x, position.y, pieceType);
    }

    public virtual bool CanMakeMove (Move move) {
        return board.previousPossibleMoves != null && board.previousPossibleMoves.Contains(move);
    }

    public abstract bool CheckForEnd (ref bool? whiteWon, bool isWhitesTurn);

    public bool CheckForEnd (ref bool? whiteWon) {
        return CheckForEnd(ref whiteWon, isWhitesTurn);
    }

    public Move getAIMove () {
        return ai.minimax();
    }

    public abstract int scoreBoard (bool isWhitesTurn);

}
