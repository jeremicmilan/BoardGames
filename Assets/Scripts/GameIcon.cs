using UnityEngine;
using System.Collections;

public class GameIcon : MonoBehaviour {

    public UI ui;

    public GameName gameName;

    public void OnClick () {
        ui.setActiveUI(ui.gameMenu);

        ui.SetGame(gameName);
    }
}
