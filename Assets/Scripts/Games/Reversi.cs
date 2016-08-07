using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reversi : Game {

    public bool isAttack;

    public Reversi ()
        : base(GameName.REVERSI, "Reversi", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(8, 8, BoardType.UNCHECKERED);

        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE }},
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.RV_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE }},
                              true);
    }
    public override void StartSinglePlayer () {
        SetBoardAndPieces();
        board.markerPossible = board.markerInvisible;
        board.markerSelected = board.markerInvisible;
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }

    public override bool Attack (Move move, bool destroy = true) {
        return true;
    }

    public void ChangeColor(Position start, Position direction) {

        Position currentPosition = start + direction;

        if (CanMoveTo(currentPosition))
            return;

        Piece piece = board.GetField(currentPosition).FindPiece();

        if (piece.isWhite != isWhitesTurn) {
            board.SendToGraveyard(piece);
            board.setPiece(PieceType.RV_PAWN, currentPosition);
            ChangeColor(currentPosition, direction);
        }
    }


    public void ChangeColor(List<Move> moves, Position end) {
        foreach (Move move in moves) {
            if (move.end == end) {
                if(move.end.x == move.start.x)
                    if(move.end.y > move.start.y)
                        ChangeColor(move.start, new Position(0, 1, null));
                    else
                        ChangeColor(move.start, new Position(0, -1, null));
                else if (move.end.y == move.start.y)
                    if (move.end.x > move.start.y)
                        ChangeColor(move.start, new Position(1, 0, null));
                    else
                        ChangeColor(move.start, new Position(-1, 0, null));
                else {
                    if (move.end.x > move.start.x && move.end.y > move.start.y)
                        ChangeColor(move.start, new Position(1, 1, null));
                    if (move.end.x < move.start.x && move.end.y < move.start.y)
                        ChangeColor(move.start, new Position(-1, -1, null));
                    if (move.end.x > move.start.x && move.end.y < move.start.y)
                        ChangeColor(move.start, new Position(1, -1, null));
                    if (move.end.x < move.start.x && move.end.y > move.start.y)
                        ChangeColor(move.start, new Position(-1, 1, null));

                }
                
            }

        }
    }

    public List<Move> returnPossibleMoves () {
        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        List<Move> possibleMoves = new List<Move> ();

        foreach(Piece piece in pieces)
            if(piece.isWhite == isWhitesTurn)
                possibleMoves.AddRange(piece.GetPawnReversiMoves());

        return possibleMoves;        
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override void MakeMove(Move move) {
  
        ChangeColor(returnPossibleMoves(), move.end);
        board.setPiece(PieceType.RV_PAWN, move.end);

        CheckForPieceEvolve(move);

        if (returnPossibleMoves().Count != 0)
            isWhitesTurn = !isWhitesTurn;

        board.ClearMarkers();
        board.moveHistory.Push(move);

        Text OnTheMove = GameObject.FindGameObjectWithTag("OnTheMove").GetComponent<Text>();
        if (isWhitesTurn)
            OnTheMove.text = "White is on the move";
        else
            OnTheMove.text = "Black is on the move";
    }

    public override void MarkFields(Position start, List<Move> possibleMoves) {
        board.ClearMarkers();

        board.MakeMarker(start, board.markerSelected);

        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> moves = piece.PossibleMoves();
                foreach (Move move in moves) {
                    board.MakeMarker(move.end, board.markerAttack);
                }
            }
        }

    }   

    public override bool CheckForEnd (ref bool? whiteWon) {
        int white = 0;
        List<Piece> pieces = board.FindAllPieces(PieceType.RV_PAWN);
        if (pieces.Count == 64) {           
            foreach (Piece piece in pieces)
                if (piece.isWhite)
                    white++;

            if (white > 32)
                whiteWon = true;
            if (white < 32)
                whiteWon = false;
            else
                whiteWon = null;

            return true;             
        }

        return false;
    }

    public override bool CheckForPieceEvolve (Move move) {
        return false;
    }
}
