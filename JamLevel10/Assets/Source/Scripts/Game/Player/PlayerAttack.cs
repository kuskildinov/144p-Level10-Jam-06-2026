using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _fireRate = 0.15f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private AttackPoint _attackPoint;
    private float _nextShotTime;
    private bool _attackPressed;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        InitializeAttackPoint();
        SubscribeToEvents();
    }

    private void Update()
    {
        if(_attackPressed)
            Shoot();
    }

    #region >>> ATTACK POINT

    private void InitializeAttackPoint()
    {
        _attackPoint = GetComponentInChildren<AttackPoint>();
        if (_attackPoint == null) { Debug.LogError("Error: Cant find AttackPoint on Player"); }
    }

    #endregion
    #region >>> ATTACK

    private void Shoot()
    {
        if (!_player.IsActive || !_player.IsAlive)
            return;

        if (Time.time < _nextShotTime)
            return;

        _nextShotTime = Time.time + _fireRate;

        Instantiate(
            _bulletPrefab,
            _attackPoint.transform.position,
            Quaternion.identity);
    }

    #endregion
    #region >>> INPUT

    private void OnAttackInputChanged(bool isPressed)
    {
        _attackPressed = isPressed;
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
