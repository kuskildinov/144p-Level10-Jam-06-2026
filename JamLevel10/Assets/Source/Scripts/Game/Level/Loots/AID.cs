using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AID : Loot
{
    [Header("References")]
    [SerializeField] private Transform _visual;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3f;

    [Header("Movement Area")]
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    [Header("Idle Animation")]
    [SerializeField] private float _floatHeight = 0.25f;
    [SerializeField] private float _floatDuration = 0.8f;
    [SerializeField] private float _rotateAmount = 8f;

    [Header("Spawn Animation")]
    [SerializeField] private float _spawnDuration = 0.35f;

    [Header("Disappear Animation")]
    [SerializeField] private float _disappearDuration = 0.4f;

    private Tween _moveTween;
    private Sequence _idleSequence;
    private Vector3 _currentTarget;

    private Vector3 _graphicsStartPos;

    private void Update()
    {
        if (_currentTarget == null)
            return;

        Move();

        if(Vector3.Distance(transform.position, _currentTarget) <= 0.1f)
        {
            _currentTarget = GetRandomPoint();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        PlaySpawnAnimation();
        StartIdleAnimation();
        _graphicsStartPos = _visual.localPosition;
        _currentTarget = GetRandomPoint();
    }

    private void OnDisable()
    {       
        _moveTween?.Kill();
        _idleSequence?.Kill();
    }
    
    private Vector3 GetRandomPoint()
    {
        return new Vector3(
            Random.Range(_minBounds.x, _maxBounds.x),
            Random.Range(_minBounds.y, _maxBounds.y),
            transform.position.z);
    }

    private void StartIdleAnimation()
    {
        _idleSequence?.Kill();

        _visual.localPosition = _graphicsStartPos;
        _visual.localRotation = Quaternion.identity;

        _idleSequence = DOTween.Sequence();

        _idleSequence.Append(
            _visual.DOLocalMoveY(_graphicsStartPos.y + _floatHeight, _floatDuration));

        _idleSequence.Join(
            _visual.DOLocalRotate(
                new Vector3(0, 0, _rotateAmount),
                _floatDuration));

        _idleSequence.Append(
            _visual.DOLocalMoveY(_graphicsStartPos.y, _floatDuration));

        _idleSequence.Join(
            _visual.DOLocalRotate(
                new Vector3(0, 0, -_rotateAmount),
                _floatDuration));

        _idleSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void PlaySpawnAnimation()
    {
        _visual.localScale = Vector3.zero;

        _visual
            .DOScale(Vector3.one, _spawnDuration)
            .SetEase(Ease.OutBack);
    }

    public void Collect()
    {
        _moveTween?.Kill();
        _idleSequence?.Kill();

        Sequence seq = DOTween.Sequence();

        seq.Append(
            _visual.DOLocalMoveY(_graphicsStartPos.y + 0.6f, _disappearDuration));

        seq.Join(
            _visual.DOScale(Vector3.zero, _disappearDuration));

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}