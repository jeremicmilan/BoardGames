using UnityEngine;
using System.Collections;

public enum BoardType { CHECKERED, CUSTOM };
public enum PieceType { PAWN, KING, ROOK, BISHOP, KINGHT, QUEEN };

public class Board : MonoBehaviour {
    
    public GameObject blackField;
    public GameObject whiteField;
    
    public void BuildBoard (int width, int height, BoardType boardType, GameObject[][] customLayout = null) {
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
                    case BoardType.CUSTOM:
                        field = customLayout[i][j];
                        break;
                    default:
                        break;
                }

                
                GameObject instantiatedField = (GameObject)Instantiate(field,
                                                                       scaleFactor * (initialPosition + sizeX * new Vector3(j, i, 0)),
                                                                       Quaternion.identity);

                instantiatedField.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                instantiatedField.transform.parent = gameObject.transform;
            }
        }
    }
    
    
}
