using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Checkers : Game {

    public Checkers ()
        : base(GameName.CHECKERS, "Checkers", "", null) { }

    void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        board.graveyard = GameObject.Find("Graveyard");

        board.BuildBoard(8, 8, BoardType.CHECKERED);


        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE },
                                           { PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE } },
                              false);
        board.SetPieces(new PieceType[,] { { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_KING, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE, PieceType.CK_PAWN },
                                           { PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.AL_NONE, PieceType.CK_PAWN, PieceType.AL_NONE } },
                              true);

    }

    public override void StartSinglePlayer () {
        SetBoardAndPieces();
    }

    public override void StartTwoPlayer () {
        SetBoardAndPieces();
    }

    public override bool Attack (Move move, bool destroy = true) {
        if (Math.Abs(move.start.x - move.end.x) == 2) {
            Piece piece = board.GetField(move.start.x + (move.end.x - move.start.x) / 2, move.start.y + (move.end.y - move.start.y) / 2).FindPiece();
            if (destroy) {
                move.eatenPieces.Add(piece);
                board.SendToGraveyard(piece);
            }
            return true; 
        }
       

        return false;
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();
        
        return !piece;
    }


    public override bool CheckForEnd (ref bool whiteWon) {
        int white = 0;
        int black = 0;

        bool whiteCanMove = false;
        bool blackCanMove = false;

        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach(Piece piece in pieces)
        {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves().Count > 0)
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves().Count > 0)
                    blackCanMove = true;
            }
        }

        pieces = board.FindAllPieces(PieceType.CK_KING);
        foreach (Piece piece in pieces) {
            if (piece.isWhite) {
                white++;
                if (piece.PossibleMoves().Count > 0)
                    whiteCanMove = true;
            } else {
                black++;
                if (piece.PossibleMoves().Count > 0)
                    blackCanMove = true;
            }
        }

        if(white == 0 || (!whiteCanMove && isWhitesTurn)) {
            whiteWon = false;
            return true;
        }
        else if(black == 0 || (!blackCanMove && !isWhitesTurn)){
            whiteWon = true;
            return true;
        }

        return false;

    }

    public void evolve(Move move, Piece piece){
        move.eatenPieces.Add(piece);
        board.SendToGraveyard(piece);
        
        GameObject newPiece = null;
        newPiece = UnityEngine.Object.Instantiate<GameObject>(board.checkersKing);
        Piece king = newPiece.GetComponent<Piece>();
        king.transform.parent = move.end.field.transform;
        king.transform.localPosition = new Vector3(0, 0, -1);
        king.transform.localScale = move.end.field.transform.localScale;

        if (!isWhitesTurn) {
            newPiece.transform.GetComponent<Renderer>().material.color = Color.gray;
        }

        king.board = board;
        king.position = move.end;
        king.game = this;
        king.isWhite = isWhitesTurn;
        
    }

    public override bool CheckForPieceEvolve (Move move) {
        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                if (isWhitesTurn && piece.position.y == 0) {
                    evolve(move, piece);
                } else if (!isWhitesTurn && piece.position.y == board.height - 1) {
                    evolve(move, piece);
                }
            }
        }

        return false;
    }

    public bool CheckForAttack() {
        
        List<Piece> pieces = board.FindAllPieces(PieceType.CK_PAWN);
        foreach(Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> possibleMoves = piece.PossibleMoves();
                foreach (Move move in possibleMoves) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }

        pieces = board.FindAllPieces(PieceType.CK_KING);
        foreach (Piece piece in pieces) {
            if (piece.isWhite == isWhitesTurn) {
                List<Move> possibleMoves = piece.PossibleMoves();
                foreach (Move move in possibleMoves) {
                    if (move.isAttack) {
                        return true;
                    }
                }
            }
        }

       
        return false;
    }

    
}
