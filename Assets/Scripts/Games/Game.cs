using UnityEngine;
using System.Collections;

public abstract class Game {

    public GameName gameName;
    public string name;
    public string description;
    public GameObject picture;

    public GameObject board;

    public bool isWhitesTurn;

    public Game (GameName gameName, string name, string description, GameObject picture) {
        this.gameName = gameName;
        this.name = name;
        this.description = name;
        this.picture = picture;
    }

    public abstract void StartSinglePlayer ();
    public abstract void StartTwoPlayer ();
}
