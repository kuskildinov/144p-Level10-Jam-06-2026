using System;
using UnityEngine;

public class GameStatesRoot : CompositeRoot
{
    private GameState _currentState;

    public event Action<GameState> GameStateChanged;

    public override void Compose()
    {       
        SetState(GameState.Gameplay);
    }

    public void SetState(GameState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case GameState.Gameplay:
                {
                    EnterGameplayState();
                    break;
                }
            case GameState.Dialog:
                {
                    EnterDialogState();
                    break;
                }
            case GameState.Pause:
                {
                    EnterPauseState();
                    break;
                }
            case GameState.CutScene:
                {
                    EnterCutSceneState();
                    break;
                }
            case GameState.Loading:
                {
                    EnterLoadingState();
                    break;
                }
        }
        GameStateChanged?.Invoke(_currentState);
    }

    private void EnterGameplayState()
    {
        Debug.Log("Gameplay режим активирован");
    }

    private void EnterDialogState()
    {
        Debug.Log("Диалог режим активирован");
    }

    private void EnterPauseState()
    {
        Debug.Log("Пауза активирована");
    }

    private void EnterCutSceneState()
    {
        Debug.Log("Начало кат сцены");
    }

    private void EnterLoadingState()
    {

    }
}

public enum GameState
{
    Gameplay,
    CutScene,
    Dialog,
    Pause,
    Loading,
}
