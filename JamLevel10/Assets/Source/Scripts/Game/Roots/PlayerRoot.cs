using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private Player _player;
    private PlayerPanel _playerPanel;
    private PlayerInputHandler _inputHandler;

    private GameStatesRoot _gameStatesRoot;
    private LevelRoot _levelRoot;

    public override void Compose()
    {
        GetOtherLinks();
        InitializeInput();
        InitializePlayer();
        InitializePlayerPanel();
        UpdateHealthPanel();
        SubscribeToEvents();
    }

    private void GetOtherLinks()
    {
        _gameStatesRoot = FindAnyObjectByType<GameStatesRoot>();
        if (_gameStatesRoot == null) Debug.LogError("Cant find GameStatesRoot on scene for PlayerRoot");

        _levelRoot = FindAnyObjectByType<LevelRoot>();
        if (_levelRoot == null) Debug.LogError("Cant find LevelRoot on scene for PlayerRoot");
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

    public void OnPlayerDead()
    {
        Destroy(_player.gameObject);
       _levelRoot.OnPlayerDead();
    }

    #endregion
    #region >>> CRISTALS

    public void OnCristalTaked()
    {
        GlovalVars.CristalCount++;
        UpdateCristalCount();
        CheckCristalCollectionComplete();
    }

    private void CheckCristalCollectionComplete()
    {
        int currentCount = GlovalVars.CristalCount;
        int maxCount = GlovalVars.CurrentMaxCristalCount;
        if(currentCount >= maxCount)
        {
            Debug.Log("Уровень пройден!");
        }
    }

    #endregion
    #region >>> PLAYER PANEL

    private void InitializePlayerPanel()
    {
        _playerPanel = FindAnyObjectByType<PlayerPanel>();
        if (_playerPanel == null) { Debug.LogError("Error: Cant find PlayerPanel on scene"); return; }
        _playerPanel.Initialize(this);
    }

    public void UpdateHealthPanel()
    {
        _playerPanel.UpdateHealthCount(_player.CurrentHP);
    }

    public void UpdateCristalCount()
    {
        _playerPanel.UpdateCristalCount();
    }

    public void SetMaxCristalCount()
    {
        _playerPanel.SetMaxCristalCount();
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
