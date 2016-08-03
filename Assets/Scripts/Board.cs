using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BoardType { CHECKERED, CUSTOM, UNCHECKERED };
public enum PieceType { CH_PAWN, CH_KING, CH_KNIG, CH_BISH, CH_ROOK, CH_QUEE,
                        VK_KING, VK_ROOK,
                        CK_PAWN, CK_KING,
                        RV_PAWN,
                        AL_NONE };

public class Board : MonoBehaviour {

    public GameObject blackField;
    public GameObject whiteField;
    public GameObject castleField;
    public GameObject escapeField;
    public GameObject neutralField;

    public int width;
    public int height;

    // pieces
    public GameObject chessPawn;
    public GameObject chessKing;
    public GameObject chessKnight;
    public GameObject chessBishop;
    public GameObject chessRook;
    public GameObject chessQueen;

    public GameObject vikingKing;
    public GameObject vikingRook;

    public GameObject checkersPawn;
    public GameObject checkersKing;

    public GameObject reversiPawn;


    [HideInInspector]
    public Field[,] board;

    public GameObject markerSelected;
    public GameObject markerPossible;
    public GameObject markerAttack;
    public GameObject markerPreviousMove;

    [HideInInspector]
    public List<GameObject> markers = new List<GameObject>();

    [HideInInspector]
    public Position previousPositionClicked;
    [HideInInspector]
    public List<Move> previousMoves;

    [HideInInspector]
    public Game game;

    public void BuildBoard (int width, int height, BoardType boardType, FieldType[,] customLayout = null) {
        this.width = width;
        this.height = height;
        board = new Field[height,width];

        Vector3 size = blackField.GetComponent<Renderer>().bounds.size;
        float sizeX = size.x;

        float scaleFactor = 1.5f * Mathf.Min(((float)Screen.width / width) / 300f, ((float)Screen.height / height) / 300f);
        Vector3 initialPosition = -sizeX * new Vector3(width / 2f, height / 2f, 0) + size / 2 + new Vector3(5, 0, 0);

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject fieldPrefab = null;

                switch (boardType) {
                    case BoardType.CHECKERED:
                        fieldPrefab = (i + j) % 2 == 1 ? blackField : whiteField;
                        break;
                    case BoardType.UNCHECKERED:
                        fieldPrefab = whiteField;
                        break;
                    case BoardType.CUSTOM:
                        switch (customLayout[j, i]) {
                            case FieldType.BLACK:
                                fieldPrefab = blackField;
                                break;
                            case FieldType.WHITE:
                                fieldPrefab = whiteField;
                                break;
                            case FieldType.CASTL:
                                fieldPrefab = castleField;
                                break;
                            case FieldType.ESCAP:
                                fieldPrefab = escapeField;
                                break;
                            case FieldType.NEUTR:
                                fieldPrefab = neutralField;
                                break;
                        }
                        break;
                    default:
                        break;
                }

                GameObject fieldObject = (GameObject)Instantiate(fieldPrefab,
                                                                 scaleFactor * (initialPosition + sizeX * new Vector3(i, j, 0)),
                                                                 Quaternion.identity);

                fieldObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                fieldObject.transform.parent = gameObject.transform;

                Field field = fieldObject.GetComponent<Field>();

                field.position = new Position(i, j, field);
                field.board = this;

                board[j, i] = field;
            }
        }
    }

    public void SetPieces (PieceType[,] pieceLayout, bool isWhite) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject pieceObject = null;

                switch(pieceLayout[i, j]) {
                    case PieceType.CH_PAWN:
                        pieceObject = Instantiate(chessPawn);
                        break;
                    case PieceType.CH_KING:
                        pieceObject = Instantiate(chessKing);
                        break;
                    case PieceType.CH_KNIG:
                        pieceObject = Instantiate(chessKnight);
                        break;
                    case PieceType.CH_BISH:
                        pieceObject = Instantiate(chessBishop);
                        break;
                    case PieceType.CH_ROOK:
                        pieceObject = Instantiate(chessRook);
                        break;
                    case PieceType.CH_QUEE:
                        pieceObject = Instantiate(chessQueen);
                        break;

                    case PieceType.VK_KING:
                        pieceObject = Instantiate(vikingKing);
                        break;
                    case PieceType.VK_ROOK:
                        pieceObject = Instantiate(vikingRook);
                        break;

                    case PieceType.CK_PAWN:
                        pieceObject = Instantiate(checkersPawn);
                        break;
                    case PieceType.CK_KING:
                        pieceObject = Instantiate(checkersKing);
                        break;

                    case PieceType.RV_PAWN:
                        pieceObject = Instantiate(reversiPawn);
                        break;

                    default:
                        continue;
                }

                Piece piece = pieceObject.GetComponent<Piece>();

                piece.transform.parent = board[j, i].transform;
                piece.transform.localPosition = new Vector3(0, 0, -1);
                piece.transform.localScale = board[j, i].transform.localScale;

                if (!isWhite) {
                    pieceObject.transform.GetComponent<Renderer>().material.color = Color.gray;
                }

                piece.board = this;
                piece.position = new Position(i, j, GetField(i, j));
                piece.game = game;
                piece.isWhite = isWhite;
            }
        }
    }

    public bool IsOcupied (int x, int y, PieceType pieceType = PieceType.AL_NONE) {
        return board[y, x].FindPiece() ||
               (board[y, x].fieldType == FieldType.CASTL && pieceType != PieceType.VK_KING);
    }

    public bool IsOcupied (Position position, PieceType pieceType = PieceType.AL_NONE) {
        return IsOcupied(position.x, position.y, pieceType);
    }

    public Field GetField (int x, int y) {
        return board[y, x];
    }

    public Field GetField (Position position) {
        return GetField(position.x, position.y);
    }

    public Piece GetPiece (int x, int y) {
        return board[y, x].FindPiece();
    }

    public Piece GetPiece (Position position) {
        return GetPiece(position.x, position.y);
    }

    public bool ValidPosition (int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool ValidPosition (Position position) {
        return ValidPosition(position.x, position.y);
    }

    public bool CanJumpOver (int x, int y) {
        return !board[y, x].FindPiece();
    }

    public bool CanJumpOver (Position position) {
        return CanJumpOver(position.x, position.y);
    }

    public void ClearMarkers () {
        foreach (GameObject marker in markers) {
            Destroy(marker);
        }
        markers.Clear();
    }

    private void MakeMarker (Position position, GameObject marker) {
        Field field = position.field;

        GameObject instantiatedMarker = Instantiate(marker);

        instantiatedMarker.transform.parent = field.transform;
        instantiatedMarker.transform.localPosition = new Vector3(0, 0, -2);
        instantiatedMarker.transform.localScale = field.transform.localScale;

        markers.Add(instantiatedMarker);
    }

    public void MarkFields (Position start, List<Move> possibleMoves) {
        ClearMarkers();

        MakeMarker(start, markerSelected);

        foreach (Move move in possibleMoves) {
            if (game.Attack(move, false)) {
                MakeMarker(move.end, markerAttack);
            } else {
                MakeMarker(move.end, markerPossible);
            }
        }
    }

    public void MarkSelected (Position start) {
        MarkFields(start, new List<Move>());
    }

    public bool CanMakeMove (Move move) {
        return previousMoves != null && previousMoves.Contains(move);
    }

    public void MakeMove (Move move) {
        Piece piece = move.start.field.FindPiece();

        piece.transform.parent = move.end.field.transform;
        piece.position = move.end;

        piece.transform.localPosition = new Vector3(0, 0, -1);

        game.Attack(move);

        game.isWhitesTurn = !game.isWhitesTurn;

        ClearMarkers();

        Text OnTheMove =  GameObject.FindGameObjectWithTag("OnTheMove").GetComponent<Text> ();
        if (game.isWhitesTurn)
            OnTheMove.text = "White is on the move";
        else
            OnTheMove.text = "Black is on the move";
    }
}
