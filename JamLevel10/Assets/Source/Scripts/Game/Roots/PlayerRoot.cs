using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private Player _player;
    private PlayerPanel _playerPanel;
    private PlayerInputHandler _inputHandler;

    private GameStatesRoot _gameStatesRoot;
    private LevelRoot _levelRoot;

    public Player Player => _player;

    public override void Compose()
    {
        GetOtherLinks();
        InitializeInput();
        InitializePlayer();
        InitializePlayerPanel();
        UpdateHealthPanel();
        ResetAbilities();
        SubscribeToEvents();

        GlobalVars.Level = 1;
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
        SoundsRoot.Instance.PlayDeadSound();
        Destroy(_player.gameObject);
       _levelRoot.OnPlayerDead();
    }

    #endregion
    #region >>> CRISTALS

    public void OnCristalTaked()
    {
        int takedCount = GlobalVars.CurrentPlayerExpMultiply;
        GlobalVars.CristalCount += takedCount;
        UpdateCristalCount();
        CheckCristalCollectionComplete();
    }

    private void CheckCristalCollectionComplete()
    {
        int currentCount = GlobalVars.CristalCount;
        int maxCount = GlobalVars.CurrentMaxCristalCount;
        if(currentCount >= maxCount)
        {
            SoundsRoot.Instance.PlayLevelUpSound();
            _levelRoot.OpenAbilityPanel();
            GlobalVars.Level++;
            _levelRoot.OnLevelChanged();
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
    #region >>> ABILITIES

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch(upgrade.type)
        {
            case UpgradeType.Damage:
                {
                    GlobalVars.CurrentPlayerDamage += 0.5f;
                    break;
                }
            case UpgradeType.FireRate:
                {
                    GlobalVars.CurrentPlayerFiraRate -= 0.05f;
                    break;
                }
            case UpgradeType.BulletCount:
                {
                    GlobalVars.CurrentPlayerBulletCount++;
                    break;
                }
            case UpgradeType.BulletSize:
                {
                    GlobalVars.CurrentPlayerBulletSize += 1f;
                    break;
                }
            case UpgradeType.ThropwForce:
                {
                    GlobalVars.CurrentPlayerBulletThrow++;
                    break;
                }
            case UpgradeType.Exp:
                {
                    GlobalVars.CurrentPlayerExpMultiply ++;
                    break;
                }
        }
    }

    public void ResetAbilities()
    {
        GlobalVars.CurrentPlayerDamage = GlobalVars.StartPlayerDamage;
        GlobalVars.CurrentPlayerFiraRate = GlobalVars.StartPlayerFiraRate;
        GlobalVars.CurrentPlayerBulletCount = GlobalVars.StartPlayerBulletCount;
        GlobalVars.CurrentPlayerBulletSize = GlobalVars.StartPlayerBulletSize;
        GlobalVars.CurrentPlayerBulletThrow = GlobalVars.StartPlayerBulletThrow;
        GlobalVars.CurrentPlayerExpMultiply = GlobalVars.StartPlayerExpMultiply;
    }

    #endregion
    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
