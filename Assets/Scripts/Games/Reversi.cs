using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reversi : Game {

    public bool isAttack;
    private List<Piece> toChange = new List<Piece> ();

    public Reversi ()
        : base(GameName.REVERSI, "Reversi") { }

    protected override void SetBoardAndPieces () {
        GameObject boardObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Board", typeof(GameObject)));

        board = boardObject.GetComponent<Board>();
        board.game = this;
        ai.board = board;
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

    protected override void GameSpecificStartSinglePlayer () {
        isWhitesTurn = false;
        board.markerPossible = board.markerInvisible;
        board.markerSelected = board.markerInvisible;
        MarkFields(new Position(-1, -1, null), returnPossibleMoves().ToList());
    }

    protected override void GameSpecificStartTwoPlayer () {
        isWhitesTurn = false;
        board.markerPossible = board.markerInvisible;
        board.markerSelected = board.markerInvisible;
        MarkFields(new Position(-1, -1, null), returnPossibleMoves().ToList());
    }

    public override bool Attack (Move move, bool destroy = true) {
        return true;
    }

    public void ChangeColor(Position pos, Position direction) {
        Position currentPosition = pos + direction;

        if (!board.ValidPosition(currentPosition) || CanMoveTo(currentPosition))
            return;

        Piece piece = board.GetField(currentPosition).FindPiece();

        if (piece.isWhite != isWhitesTurn) {
            isAttack = true;
            toChange.Add(piece);
            ChangeColor(currentPosition, direction);
        }

        if (piece.isWhite == isWhitesTurn && isAttack) {
            foreach (Piece p in toChange) {
                board.setPiece(PieceType.RV_PAWN, isWhitesTurn, p.position);
                board.SendToGraveyard(p);
            }
        }
    }


    public void ChangeColor(Move move) {
        Position end = move.end;

        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(0, 1, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(0, -1, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, 0, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, 0, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, 1, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, -1, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(1, -1, null));
        move.eatenPieces.AddRange(toChange);
        toChange.Clear(); isAttack = false;
        ChangeColor(end, new Position(-1, 1, null));
        move.eatenPieces.AddRange(toChange);
    }

    public IEnumerable<Move> returnPossibleMoves () {
        foreach (Piece piece in board.GetPieces(PieceType.RV_PAWN))
            if (piece.isWhite == isWhitesTurn)
                foreach (Move move in piece.GetPawnReversiMoves())
                    yield return move;
    }

    public override bool CanMakeMove(Move move) {
        foreach (Move m in returnPossibleMoves())
            if (m.end == move.end)
                return true;
        return false;
    }

    public override bool CanMoveTo (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        Field field = board.GetField(x, y);
        Piece piece = field.FindPiece();

        return !piece;
    }

    public override void MakeMove(Move move, bool fake = false) {
        ChangeColor(move);
        board.setPiece(PieceType.RV_PAWN, isWhitesTurn, move.end);

        isWhitesTurn = !isWhitesTurn;

        if (!returnPossibleMoves().Any())
            isWhitesTurn = !isWhitesTurn;

        board.ClearMarkers();
        board.moveHistory.Push(move);
        board.UpdatePlayerStatusText();

        MarkFields(new Position(-1,-1,null), returnPossibleMoves().ToList());
    }

    public override void UndoMove(Move move, bool fake = false) {
        board.SendToGraveyard(move.end.field.FindPiece());

        foreach (Piece p in move.eatenPieces) {
            board.SendToGraveyard(p.position.field.FindPiece());
            p.transform.parent = p.position.field.transform;
            p.gameObject.SetActive(true);
            p.transform.localPosition = new Vector3(0, 0, -1);
        }

        isWhitesTurn = !isWhitesTurn;

        gameEnded = false;
        board.UpdateGameStatusText("");

        if (!returnPossibleMoves().Any())
            isWhitesTurn = !isWhitesTurn;

        if (!fake) {
            board.UpdatePlayerStatusText();
            board.ClearMarkers();
            MarkFields(new Position(-1, -1, null), returnPossibleMoves().ToList());
        }
    }

    public override void MarkFields(Position start, List<Move> possibleMoves) {
        board.ClearMarkers();

        if (possibleMoves.Count == 0)
            possibleMoves.AddRange(returnPossibleMoves());

        foreach (Piece piece in board.GetPieces(PieceType.RV_PAWN)) {
            if (piece.isWhite == isWhitesTurn) {
                foreach (Move move in piece.PossibleMoves(isWhitesTurn)) {
                    bool set = false;

                    foreach (GameObject marker in board.markers)
                        if (marker.transform.parent == move.end.field.transform)
                            set = true;

                    if (!set)
                        board.MakeMarker(move.end, board.markerAttack);
                }
            }
        }
    }

    public bool NoMoreMoves() {
        bool result = false;
        if (!returnPossibleMoves().Any()) {
            isWhitesTurn = !isWhitesTurn;
            if (!returnPossibleMoves().Any())
                result = true;
            isWhitesTurn = !isWhitesTurn;
        }

        return result;
    }

    public override bool CheckForEnd (ref bool? whiteWon, bool isWhitesTurn) {
        int white = 0;
        int black = 0;
        List<Piece> pieces = board.GetPieces(PieceType.RV_PAWN).ToList();
        if (pieces.Count == 64 || NoMoreMoves()) {
            foreach (Piece piece in pieces) {
                if (piece.isWhite)
                    white++;
                else
                    black++;
            }

            if (white > black)
                whiteWon = true;
            else if (white < black)
                whiteWon = false;
            else
                whiteWon = null;

            return true;
        }

        return false;
    }
}
