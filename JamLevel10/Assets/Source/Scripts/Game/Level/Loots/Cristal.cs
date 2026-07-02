using UnityEngine;

public class Cristal : Loot
{
    [SerializeField] private float _magnetRadius = 4f;
    [SerializeField] private float _startSpeed = 3f;
    [SerializeField] private float _acceleration = 12f;
    [SerializeField] private float _collectDistance = 0.2f;

    private Transform _player;
    private bool _isMagnetized;
    private float _currentSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();

        _player = FindFirstObjectByType<Player>().transform;
        _currentSpeed = _startSpeed;
    }

    private void Update()
    {
        if (!_isMagnetized)
        {
            float sqrDistance =
                (_player.position - transform.position).sqrMagnitude;

            if (sqrDistance <= _magnetRadius * _magnetRadius)
            {
                _isMagnetized = true;
            }

            return;
        }

        _currentSpeed += _acceleration * Time.deltaTime;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _player.position,
            _currentSpeed * Time.deltaTime);
               
    }
}
