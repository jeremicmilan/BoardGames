using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameList : MonoBehaviour {

    public List<Game> gameList;

	void Start () {
        gameList = new List<Game> { new Game("Viking chess", "", null),
                                    new Game("Viking chess", "", null) };

	    foreach (Game game in gameList) {

        }
	}
}
