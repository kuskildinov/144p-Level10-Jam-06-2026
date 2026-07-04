using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerBullet _bulletPrefab;
    [SerializeField] private float _fireRate = 0.15f;   

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private AttackPoint _attackPoint;
    private float _nextFireTime;
    private bool _attackPressed;
    private float _spreadAngle = 30f;

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

    public void Shoot()
    {
        int bulletCount = GlobalVars.CurrentPlayerBulletCount;
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + GlobalVars.CurrentPlayerFiraRate;
        SoundsRoot.Instance.PlayAttackSound();

        if (bulletCount <= 1)
        {
            SpawnBullet(_attackPoint.transform.forward);
            return;
        }

        float step = _spreadAngle / (bulletCount - 1);
        float startAngle = -_spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + step * i;

            Vector3 dir = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f
            );

           
            SpawnBullet(dir);
        }
    }

    private void SpawnBullet(Vector3 direction)
    {
        PlayerBullet bullet = Instantiate(
            _bulletPrefab,
            _attackPoint.transform.position,
            Quaternion.identity
        );

        float scale = GlobalVars.CurrentPlayerBulletSize;
        bullet.transform.localScale = Vector3.one * (1f + scale);
        bullet.Init(direction);
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
