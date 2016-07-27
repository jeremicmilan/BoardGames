﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
    
    public Game[] gameList = new Game[] { new VikingChess(), new Checkers(), new Reversi() };
    public Game game;

    public Text gameTitle;

    private Game FindGame (GameName gameName) {
        foreach (Game game in gameList) {
            if (game.gameName == gameName) {
                return game;
            }
        }
        return null;
    }

    public void SetGame (GameName gameName) {
        game = FindGame(gameName);

        gameTitle.text = game.name;
    }

    public void StartSinglePlayer () {
        UI ui = GameObject.Find("UI").GetComponent<UI>();
        ui.setActiveUI(GameObject.FindGameObjectWithTag("game ui").GetComponent<Canvas>());

        game.StartSinglePlayer();
    }

    public void StartTwoPlayer () {
        UI ui = GameObject.Find("UI").GetComponent<UI>();
        ui.setActiveUI(GameObject.FindGameObjectWithTag("game ui").GetComponent<Canvas>());

        game.StartTwoPlayer();
    }
}
