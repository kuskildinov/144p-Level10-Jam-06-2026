using DG.Tweening;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float _maxTilt = 20f;
    [SerializeField] private float _tiltDuration = 0.15f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Tween _shakeTween;
    private Tween _scaleTween;

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
   

    public void SetMiniMode(bool mini)
    {
        _scaleTween?.Kill();

        if (mini)
        {
            _scaleTween = _player.transform.DOScale(0.6f, 0.1f);
        }
        else
        {
            _scaleTween = _player.transform.DOScale(1f, 0.1f);
        }
    }

    #region >>> INPUT
   
    private void OnAttackInputChanged(bool isPressed)
    {
      
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.AttackInput += OnAttackInputChanged;       
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.AttackInput -= OnAttackInputChanged;       
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}
