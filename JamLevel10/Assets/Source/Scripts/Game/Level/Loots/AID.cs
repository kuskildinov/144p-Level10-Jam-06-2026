using DG.Tweening;
using UnityEngine;

public class AID : Loot
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3f;

    [Header("Movement Area")]
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    private Tween _moveTween;   
    private Vector3 _currentTarget;

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

    protected override void OnEnable()
    {
        base.OnEnable();
        _currentTarget = GetRandomPoint();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _moveTween?.Kill();       
    }
    
    private Vector3 GetRandomPoint()
    {
        return new Vector3(
            Random.Range(_minBounds.x, _maxBounds.x),
            Random.Range(_minBounds.y, _maxBounds.y),
            transform.position.z);
    }

    public override void Collect()
    {
        _moveTween?.Kill();
        base.Collect();
    }
}