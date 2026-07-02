using DG.Tweening;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform _visual;
    [Header("Idle Animation")]
    [SerializeField] private float _floatHeight = 0.25f;
    [SerializeField] private float _floatDuration = 0.8f;
    [SerializeField] private float _rotateAmount = 8f;

    [Header("Spawn Animation")]
    [SerializeField] private float _spawnDuration = 0.35f;

    [Header("Disappear Animation")]
    [SerializeField] private float _disappearDuration = 0.4f;

    protected Sequence _idleSequence;
    protected Vector3 _graphicsStartPos;

    public LootType Type;
    public bool IsBusy;

    protected virtual void OnEnable()
    {
        PlaySpawnAnimation();
        StartIdleAnimation();
        _graphicsStartPos = _visual.localPosition;      
    }

    protected virtual void OnDisable()
    {       
        _idleSequence?.Kill();
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

    public virtual void Collect()
    {       
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