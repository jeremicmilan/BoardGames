using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;

public enum GameName { VIKING_CHESS, REVERSI, CHECKERS, CHESS, FOX_AND_HOUNDS };

public class UI : MonoBehaviour {
    public Game game;

    public Canvas mainMenu;
    public Canvas gameMenu;
    public Canvas gameUI;

    public Text gameTitle;
    public Text description;
    public Image image;


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

    public void SetGame(GameName gameName) {
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
            case GameName.CHESS:
                game = new Chess();
                break;
            case GameName.FOX_AND_HOUNDS:
                game = new FoxAndHounds();
                break;
            default:
                break;
        }

        gameTitle.text = game.name;
        image.sprite = Resources.Load<Sprite>("Images/" + game.name);

        description.text = ((TextAsset)Resources.Load("Descriptions/" + game.name, typeof(TextAsset))).text; 
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

