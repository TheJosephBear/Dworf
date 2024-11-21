using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerSelectLogic : Singleton<PlayerSelectLogic> {
    public GameObject selectingUI;
    public Transform PlayerOneSpriteTransform;
    public Transform PlayerTwoSpriteTransform;
    GameObject p1SelectSpriteInstantiated;
    GameObject p2SelectSpriteInstantiated;
    int p1selectedCharacterIndex = 0;
    int p2selectedCharacterIndex = 1;

    protected override void Awake() {
        base.Awake();

        selectingUI.SetActive(false);
    }

    public void Initialize() {
        p1selectedCharacterIndex = 0;
        p2selectedCharacterIndex = 1;
        GameManager.Instance.ChangeGameState(GameState.PlayerSelect);
        selectingUI.SetActive(true);
        // default characters
        PlayerManager.Instance.PlayerOne.PlayerCharacter = PlayerManager.Instance.characterList[p1selectedCharacterIndex];
        ChangePlayerSprite(true);
        PlayerManager.Instance.PlayerTwo.PlayerCharacter = PlayerManager.Instance.characterList[p2selectedCharacterIndex];
        ChangePlayerSprite(false);
    }

    void StartGame() {
        // Trigger game start
        FindAnyObjectByType<GamePlayLogic>().StartGame();
    }

    public void ChangePlayerCharacter(bool playerOne, bool next) {
        // Add time out for each player because it can be called continuosly
        if (playerOne) {
            p1selectedCharacterIndex = CalculateIndex(playerOne, next);
            PlayerManager.Instance.PlayerOne.PlayerCharacter = PlayerManager.Instance.characterList[p1selectedCharacterIndex];
            ChangePlayerSprite(playerOne);
        } else {
            p2selectedCharacterIndex = CalculateIndex(playerOne, next);
            PlayerManager.Instance.PlayerTwo.PlayerCharacter = PlayerManager.Instance.characterList[p2selectedCharacterIndex];
            ChangePlayerSprite(playerOne);
        }
    }

    public void ConfirmSelection(bool playerOne) {
        if (playerOne) {
            PlayerManager.Instance.PlayerOne.isReady = true;
        } else {
            PlayerManager.Instance.PlayerTwo.isReady = true;
        }
        // Start countdown if it didnt start already
        FindAnyObjectByType<StartScreenLogic>().OnPlayerJoin(playerOne);
    }

    void ChangePlayerSprite(bool playerOne) {
        if (playerOne) {
            if (p1SelectSpriteInstantiated != null) Destroy(p1SelectSpriteInstantiated);
            p1SelectSpriteInstantiated = Instantiate(PlayerManager.Instance.PlayerOne.PlayerCharacter.characterSelectSprite, PlayerOneSpriteTransform.position, Quaternion.identity);
        } else {
            if (p2SelectSpriteInstantiated != null) Destroy(p2SelectSpriteInstantiated);
            p2SelectSpriteInstantiated = Instantiate(PlayerManager.Instance.PlayerTwo.PlayerCharacter.characterSelectSprite, PlayerTwoSpriteTransform.position, Quaternion.identity);
        }
    }

    int CalculateIndex(bool playerOne, bool next) {
        int index = 0;
        int characterCount = PlayerManager.Instance.characterList.Count;
        if (playerOne) {
            index = p1selectedCharacterIndex;
            if (next) {
                if (index + 1 > characterCount - 1) {
                    index = 0;
                } else {
                    index += 1;
                }
            } else {
                if (index - 1 < 0) {
                    index = characterCount - 1;
                } else {
                    index -= 1;
                }
            }
        } else {
            index = p2selectedCharacterIndex;
            if (next) {
                if (index + 1 > characterCount - 1) {
                    index = 0;
                } else {
                    index += 1;
                }
            } else {
                if (index - 1 < 0) {
                    index = characterCount - 1;
                } else {
                    index -= 1;
                }
            }
        }
        print("INDEX IS " + index);
        return index;
    }

    public void DeletePlayerPreviews() {
        if (p2SelectSpriteInstantiated != null) Destroy(p2SelectSpriteInstantiated);
        if (p1SelectSpriteInstantiated != null) Destroy(p1SelectSpriteInstantiated);
    }

}
