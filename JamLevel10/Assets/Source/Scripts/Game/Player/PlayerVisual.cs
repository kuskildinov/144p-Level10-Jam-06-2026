using DG.Tweening;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float _maxTilt = 20f;
    [SerializeField] private float _tiltDuration = 0.15f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Tween _shakeTween;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        IdleShake();
        SubscribeToEvents();
    }
    
    private void IdleShake()
    {
        _shakeTween = transform
        .DOLocalMoveY(0.1f, 0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

    #region >>> INPUT

    private void OnDashInputChanged()
    {
        _player.TryDash();
    }

    private void OnAttackInputChanged(bool isPressed)
    {
      
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.AttackInput += OnAttackInputChanged;
        _inputHandler.DashInput += OnDashInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.AttackInput -= OnAttackInputChanged;
        _inputHandler.DashInput -= OnDashInputChanged;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
