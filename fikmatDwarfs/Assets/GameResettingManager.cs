using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResettingManager : Singleton<GameResettingManager> {
    public void ResetGame() {
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine() {
        GameManager.Instance.ChangeGameState(GameState.ButtonIsPushed);
        var loadTask = SceneLoadingManager.Instance.ResetSceneAsync(SceneType.MainMenu, 0f);
        yield return new WaitUntil(() => loadTask.IsCompleted);
        if (loadTask.Result) {
            print("reseting done");
        } else {
            Debug.LogError("Failed to load main menu scene.");
        }
    }
}
