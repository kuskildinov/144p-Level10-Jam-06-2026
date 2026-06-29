using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private Player _player;
    private PlayerInputHandler _inputHandler;

    public override void Compose()
    {
        InitializeInput();
        InitializePlayer();
    }

    #region >>> PLAYER
    private void InitializePlayer()
    {
        _player = FindAnyObjectByType<Player>();
        if (_player == null) { Debug.LogError("Error: Cant find Player on scene"); return; }
        _player.Initialize(this, _inputHandler);
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
}
