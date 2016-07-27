using UnityEngine;
using System.Collections;

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

    public GameObject[,] board;

    public void BuildBoard (int width, int height, BoardType boardType, GameObject[,] customLayout = null) {
        this.width = width;
        this.height = height;
        board = new GameObject[height,width];

        Vector3 size = blackField.GetComponent<Renderer>().bounds.size;
        float sizeX = size.x;
        Debug.Log(size);
        float scaleFactor = 1.5f * Mathf.Min(((float)Screen.width / width) / 300f, ((float)Screen.height / height) / 300f);
        Vector3 initialPosition = -sizeX * new Vector3(width / 2f, height / 2f, 0) + size / 2;
        
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                GameObject field = null;

                switch (boardType) {
                    case BoardType.CHECKERED:
                        field = (i + j) % 2 == 1 ? blackField : whiteField;
                        break;
                    case BoardType.UNCHECKERED:
                        field = whiteField;
                        break;
                    case BoardType.CUSTOM:
                        field = customLayout[i, j];
                        break;
                    default:
                        break;
                }
                
                GameObject instantiatedField = (GameObject)Instantiate(field,
                                                                       scaleFactor * (initialPosition + sizeX * new Vector3(j, i, 0)),
                                                                       Quaternion.identity);

                instantiatedField.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                instantiatedField.transform.parent = gameObject.transform;

                board[i, j] = instantiatedField;
            }
        }
    }

    public void SetPieces (PieceType[,] pieceLayout, bool isWhite) {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
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

                piece.transform.parent = board[i, j].transform;
                piece.transform.localPosition = new Vector3(0, 0, -1);
                piece.transform.localScale = board[i, j].transform.localScale;

                if (!isWhite)
                    piece.transform.GetComponent<Renderer>().material.color = Color.gray;

                
            }
        }
    }
    
}
