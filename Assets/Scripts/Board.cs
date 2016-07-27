using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoardType { CHECKERED, CUSTOM, UNCHECKERED };
public enum PieceType { PAWN, KING, ROOK, BISHOP, KINGHT, QUEEN, NONE };

public class Board : MonoBehaviour {

    public GameObject blackField;
    public GameObject whiteField;

    public int width;
    public int height;

    public GameObject rook;
    public GameObject king;
    public GameObject queen;
    public GameObject bishop;
    public GameObject pawn;
    public GameObject knight;

    [HideInInspector]
    public GameObject[,] board;

    public GameObject markerSelected;
    public GameObject markerPossible;
    public GameObject markerAttack;

    [HideInInspector]
    public List<GameObject> markers = new List<GameObject>();

    public void BuildBoard (int width, int height, BoardType boardType, GameObject[,] customLayout = null) {
        this.width = width;
        this.height = height;
        board = new GameObject[height,width];

        Vector3 size = blackField.GetComponent<Renderer>().bounds.size;
        float sizeX = size.x;

        float scaleFactor = 1.5f * Mathf.Min(((float)Screen.width / width) / 300f, ((float)Screen.height / height) / 300f);
        Vector3 initialPosition = -sizeX * new Vector3(width / 2f, height / 2f, 0) + size / 2;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject field = null;

                switch (boardType) {
                    case BoardType.CHECKERED:
                        field = (i + j) % 2 == 1 ? blackField : whiteField;
                        break;
                    case BoardType.UNCHECKERED:
                        field = whiteField;
                        break;
                    case BoardType.CUSTOM:
                        field = customLayout[j, i];
                        break;
                    default:
                        break;
                }

                GameObject instantiatedField = (GameObject)Instantiate(field,
                                                                       scaleFactor * (initialPosition + sizeX * new Vector3(i, j, 0)),
                                                                       Quaternion.identity);

                instantiatedField.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                instantiatedField.transform.parent = gameObject.transform;
                instantiatedField.GetComponent<Field>().position = new Position(i, j, instantiatedField.GetComponent<Field>());

                board[j, i] = instantiatedField;
            }
        }
    }

    public void SetPieces (PieceType[,] pieceLayout, bool isWhite) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject piece = null;

                switch(pieceLayout[i, j]) {
                    case PieceType.ROOK:
                        piece = Instantiate(rook);
                        break;
                    case PieceType.KING:
                        piece = Instantiate(king);
                        break;
                    default:
                        continue;
                }
                piece.transform.parent = board[j, i].transform;
                piece.transform.localPosition = new Vector3(0, 0, -1);
                piece.transform.localScale = board[j, i].transform.localScale;

                if (!isWhite) {
                    piece.transform.GetComponent<Renderer>().material.color = Color.gray;
                }

                Piece pieceScript = piece.GetComponent<Piece>();

                pieceScript.board = this;
                pieceScript.position = new Position(i, j, GetField(i, j));
            }
        }
    }

    public bool IsOcupied (int x, int y) {
        return board[y, x].GetComponent<Field>().FindPiece();
    }

    public bool IsOcupied (Position position) {
        return IsOcupied(position.x, position.y);
    }

    public Field GetField (int x, int y) {
        return board[y, x].GetComponent<Field>();
    }

    public Field GetField (Position position) {
        return GetField(position.x, position.y);
    }

    public bool CanJumpOver (int x, int y) {
        return false;
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
            if (move.end.field.FindPiece()) {
                MakeMarker(move.end, markerAttack);
            } else {
                MakeMarker(move.end, markerPossible);
            }
        }
    }

    public void MarkSelected (Position start) {
        MarkFields(start, new List<Move>());
    }
}
