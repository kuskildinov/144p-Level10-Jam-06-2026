using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float _maxTilt = 20f;
    [SerializeField] private float _tiltDuration = 0.15f;
    [SerializeField] private Animator _animator;
    [Header("TakeDamage")]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _takeDamageMaterial;
    [SerializeField] private Material _inviseFrameMaterial;
    [Header("Dead Effect")]
    [SerializeField] private DeadEffect _deadEffectPrefab;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private bool _inviseFrameActive;
    private Tween _shakeTween;
    private Tween _scaleTween;
    protected Coroutine _takeDamageCoroutine;
    protected Coroutine _invisFrameCoroutine;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        IdleShake();
        SubscribeToEvents();
    }

    private void Update()
    {
        
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
            _animator.speed = 3f;


        }
        else
        {
            _scaleTween = _player.transform.DOScale(1f, 0.1f);
            _animator.speed = 1f;
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

    public void OnDead()
    {
        Instantiate(_deadEffectPrefab, transform.position, Quaternion.identity);
    }

    public void ShowInvisFrameBlinks()
    {
        if (_invisFrameCoroutine != null)
        {
            StopCoroutine(_invisFrameCoroutine);
            SetNormalMaterial();
        }
        _invisFrameCoroutine = StartCoroutine(InvisFrameVisualRoutine());
    }

    public void HideInvisFrameBlinks()
    {
        StopCoroutine(_invisFrameCoroutine);
        SetNormalMaterial();
    }

    private void SetNormalMaterial()
    {
        Material[] mats = _renderer.materials;
        mats[0] = _mainMaterial;
        _renderer.materials = mats;
    }

    private void SetInvisFrameMaterial()
    {
        Material[] mats = _renderer.materials;
        mats[0] = _inviseFrameMaterial;
        _renderer.materials = mats;
    }

    protected IEnumerator TakeDamageVisualRoutine()
    {
        Material[] mats = _renderer.materials;

        mats[0] = _takeDamageMaterial;
        _renderer.materials = mats;

        yield return new WaitForSecondsRealtime(0.1f);
    }

    protected IEnumerator InvisFrameVisualRoutine()
    {       
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            SetInvisFrameMaterial();
            yield return new WaitForSecondsRealtime(0.1f);
            SetNormalMaterial();
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
