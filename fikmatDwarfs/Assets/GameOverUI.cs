using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : UIBehaviour {
    public override void Hide() {
        canvas.SetActive(false);
    }

    public override void Show() {
        canvas.SetActive(false);
    }

}
