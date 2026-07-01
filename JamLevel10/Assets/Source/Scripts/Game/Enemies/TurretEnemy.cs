using UnityEngine;

public class TurretEnemy : Enemy
{
    [SerializeField] private Transform attackPosition;

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireRate = 1f;

    private float _nextShotTime;

    private enum State
    {
        MoveToPosition,
        Attack
    }

    private State _state;

    private void Start()
    {
        _state = State.MoveToPosition;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.MoveToPosition:
                MoveToPosition();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    private void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            attackPosition.position,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(
                transform.position,
                attackPosition.position) < 0.1f)
        {
            _state = State.Attack;
        }
    }

    private void Attack()
    {
        if (Time.time < _nextShotTime)
            return;

        _nextShotTime = Time.time + fireRate;

        Player player =
            FindFirstObjectByType<Player>();

        Vector2 direction =
            (player.transform.position -
             firePoint.position).normalized;

        EnemyBullet bullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity);

        bullet.Initialize(direction);
    }
}