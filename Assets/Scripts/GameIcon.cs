using UnityEngine;
using System.Collections;

public class GameIcon : MonoBehaviour {

    public UI ui;
    public GameMenu gameMenu;

    public GameName gameName;

    public void OnClick () {
        ui.setActiveUI(gameMenu.gameObject.GetComponent<Canvas>());

        gameMenu.SetGame(gameName);
    }
}
