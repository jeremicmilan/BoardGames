using UnityEngine;
using System.Collections;

public enum GameName { VIKING_CHESS, REVERSI, CHECKERS };

public class UI : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas gameMenu;
    public Canvas gameUI;

    public void setActiveUI (Canvas ui) {
        mainMenu.enabled = false;
        gameMenu.enabled = false;
        gameUI.enabled = false;

        ui.enabled = true;
    }

    public void exitApp () {
        Application.Quit();
    }

    public void QuitGame () {
        Destroy(GameObject.FindGameObjectWithTag("board"));

        setActiveUI(mainMenu);
    }

}

