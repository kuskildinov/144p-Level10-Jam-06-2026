using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private Player _player;
    private PlayerInputHandler _inputHandler;

    private GameStatesRoot _gameStatesRoot;

    public override void Compose()
    {
        GetOtherLinks();
        InitializeInput();
        InitializePlayer();
        SubscribeToEvents();
    }

    private void GetOtherLinks()
    {
        _gameStatesRoot = FindAnyObjectByType<GameStatesRoot>();
        if (_gameStatesRoot == null) Debug.LogError("Cant find GameStatesRoot on scene for PlayerRoot");
    }

    #region >>> PLAYER

    private void InitializePlayer()
    {
        _player = FindAnyObjectByType<Player>();
        if (_player == null) { Debug.LogError("Error: Cant find Player on scene"); return; }
        _player.Initialize(this, _inputHandler);
    }

    private void TogglePlayerActivation(bool value)
    {
        _player.ToggleActivation(value);
    }

    #endregion
    #region >>> INPUT

    private void InitializeInput()
    {
        _inputHandler = FindAnyObjectByType<PlayerInputHandler>();
        if (_inputHandler == null)
        {
            Debug.LogError("Error: Cant find PlayerInputHandler on scene!");
            return;
        }
    }

    #endregion
    #region >>> GAME STATE

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.Gameplay)
        {
            TogglePlayerActivation(true);
        }
        else
        {
            TogglePlayerActivation(false);
        }
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _gameStatesRoot.GameStateChanged += OnGameStateChanged;
    }

    private void UnsubscribeToEvents()
    {
        _gameStatesRoot.GameStateChanged -= OnGameStateChanged;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
