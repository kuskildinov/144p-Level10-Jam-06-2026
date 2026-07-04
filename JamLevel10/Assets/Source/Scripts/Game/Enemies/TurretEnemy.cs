using UnityEngine;

public class TurretEnemy : Enemy
{
    private const string AnimatorAttackParam = "Attack";

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [Header("Visual")]
    [SerializeField] private Animator _animator;
    [Header("Sounds")]
    [SerializeField] protected AudioSource _source;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _dieSound;

    private float _nextShotTime;

    private enum State
    {
        MoveToPosition,
        Attack
    }

    private State _state;

    public override void Initialize(EnemiesRoot root, Transform attackPoint)
    {
        base.Initialize(root, attackPoint);

        attackPosition = attackPoint;
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
            _animator.SetBool(AnimatorAttackParam, true);
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

        PlayAttackSound();
        EnemyBullet bullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity);

        bullet.Initialize(direction);
    }

    protected override void Die()
    {
        base.Die();
        PlayDieSound();
    }

    #region >>> SOUNDS

    private void PlayAttackSound()
    {
        _source.PlayOneShot(_attackSound);
    }

    private void PlayDieSound()
    {
        _source.PlayOneShot(_dieSound);
    }

    #endregion
}