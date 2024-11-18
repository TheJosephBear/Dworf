using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    //observer ew
    List<IObserver> observers = new List<IObserver>();
    public void AddObserver(IObserver observer) {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer) {
        observers.Remove(observer);
    }
    protected void NotifyObservers<T>(T data) {
        foreach (IObserver o in observers) {
            o.OnNotify(data);
        }
    }

    GameState previousState;
    GameState currentState;

    protected override void Awake() {
        base.Awake();
    }


    public void ChangeGameState(GameState state) {
        previousState = currentState;
        currentState = state;
        NotifyObservers(currentState);
    }

    public void ChangeToPreviousGameState() {
        ChangeGameState(previousState);
    }
}

public enum GameState {
    IntroCutscene,
    ButtonIsPushed,
    SecondChance,
    GameOver
}