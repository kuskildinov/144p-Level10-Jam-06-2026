using UnityEngine;

public class KamikazeEnemy : Enemy
{
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _chargeSpeed = 10f;
    [SerializeField] private float _waitBeforeCharge = 2f;
    [Header("Visual")]
    [SerializeField] private GameObject _commonModel;
    [SerializeField] private GameObject _screamModel;
    [Header("Attack Settings")]
    [SerializeField] private EnemyBullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    private Vector3 _chargeTarget;
    private Vector3 _chargeDirection;
    private float _timer;

    private enum State
    {
        MoveToPosition,
        Wait,
        Charge
    }

    private State _state;

    private void Start()
    {
        _state = State.MoveToPosition;
        _commonModel.gameObject.SetActive(true);
        _screamModel.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (_state)
        {
            case State.MoveToPosition:
                MoveToPosition();
                break;

            case State.Wait:
                Wait();
                break;

            case State.Charge:
                Charge();
                break;
        }
    }

    private void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _attackPosition.position,
            _moveSpeed * Time.deltaTime);

        if (Vector2.Distance(
                transform.position,
                _attackPosition.position) < 0.1f)
        {
            _state = State.Wait;
            _timer = _waitBeforeCharge;
        }
    }

    private void Wait()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0)
            return;

        Player player =
            FindAnyObjectByType<Player>();

        _chargeTarget = player.transform.position;

        StartCharge();

        _commonModel.gameObject.SetActive(false);
        _screamModel.gameObject.SetActive(true);

        // Тут проигрываем крик
    }

    private void StartCharge()
    {
        Player player =
            FindFirstObjectByType<Player>();

        _chargeTarget = player.transform.position;

        _chargeDirection =
            (_chargeTarget - transform.position).normalized;

        _state = State.Charge;
    }

    private void Charge()
    {
        transform.position +=
         _chargeDirection *
         _chargeSpeed *
         Time.deltaTime;

        if (transform.position.x <= _leftDestroyX || transform.position.y <= _bottomDestroyY || transform.position.y >= _topDestroyY)
        {
            Destroy(gameObject);
        }
    }

    protected override void Die()
    {
        Explode();
        base.Die();
    }

    private void SpawnRadialBullets()
    {
        const int bulletCount = 8;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);

            Vector3 direction =
                Quaternion.Euler(0f, 0f, angle) *
                Vector3.right;

            EnemyBullet bullet =
                Instantiate(
                    _bulletPrefab,
                    _bulletSpawnPoint.position,
                    Quaternion.identity);

            bullet.Initialize(direction);
        }
    }

    private void Explode()
    {
        SpawnRadialBullets();
    }
}
