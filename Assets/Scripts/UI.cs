using UnityEngine;
using System.Collections;

public enum GameName { VIKING_CHESS, REVERSI, CHECKERS };

public class UI : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject gameMenu;

    public GameName gameName;

    public void setActiveUI (GameObject ui) {
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);

        ui.SetActive(true);
    }

    public void setGameName (GameName gameName) {
        this.gameName = gameName;
    }
}
