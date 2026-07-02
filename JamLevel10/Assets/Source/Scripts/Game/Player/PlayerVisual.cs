using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float _maxTilt = 20f;
    [SerializeField] private float _tiltDuration = 0.15f;
    [Header("TakeDamage")]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _takeDamageMaterial;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Tween _shakeTween;
    private Tween _scaleTween;
    protected Coroutine _takeDamageCoroutine;

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

    public void OnTakeDamage()
    {
        if (_takeDamageCoroutine != null)
        {
            StopCoroutine(_takeDamageCoroutine);
            _renderer.materials[0] = _mainMaterial;
        }
        _takeDamageCoroutine = StartCoroutine(TakeDamageVisualRoutine());
    }

    protected IEnumerator TakeDamageVisualRoutine()
    {
        Material[] mats = _renderer.materials;

        mats[0] = _takeDamageMaterial;
        _renderer.materials = mats;

        yield return new WaitForSecondsRealtime(0.1f);

        mats[0] = _mainMaterial;
        _renderer.materials = mats;
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
