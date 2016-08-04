using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameName { VIKING_CHESS, REVERSI, CHECKERS };

public class UI : MonoBehaviour {
    public Game game;

    public Canvas mainMenu;
    public Canvas gameMenu;
    public Canvas gameUI;

    public Text gameTitle;

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

    public void SetGame (GameName gameName) {
        switch (gameName) {
            case GameName.VIKING_CHESS:
                game = new VikingChess();
                break;
            case GameName.REVERSI:
                game = new Reversi();
                break;
            case GameName.CHECKERS:
                game = new Checkers();
                break;
            default:
                break;
        }

        gameTitle.text = game.name;
    }

    public void StartSinglePlayer () {
        setActiveUI(gameUI);

        game.StartSinglePlayer();
    }

    public void StartTwoPlayer () {
        setActiveUI(gameUI);

        game.StartTwoPlayer();
    }

    public void Undo () {
        game.board.Undo();
    }
}

