using UnityEngine;
using System.Collections;

public class GameIcon : MonoBehaviour {

    public GameName gameName;

    void OnMouseDown () {
        UI ui = GameObject.FindGameObjectWithTag("ui").GetComponent<UI>();

        ui.setActiveUI(GameObject.FindGameObjectWithTag("game menu"));

        ui.gameName = gameName;
        Debug.Log("pozvana");
    }
}
