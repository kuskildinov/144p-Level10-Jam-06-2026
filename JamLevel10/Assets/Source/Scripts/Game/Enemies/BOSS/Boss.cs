using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _idleTime = 3f;
    [SerializeField] private int _health = 100;
    [Header("Visual")]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _takeDamageMaterial;
    [Header("Dead Effect")]
    [SerializeField] private DeadEffect _deadEffectPrefab;

    private Player _player;
    private BossHand _hand;
    private BossGunAttack _gunAttack;
    private BossWallAttack _wallAttack;
    private BossLaserAttack _laserAttack;

    private bool _isDead;
    private Coroutine _attackRoutine;
    private Coroutine _takeDamageCoroutine;

    public Player Player => _player;

    public void Initialzie()
    {
        GetOtherLinks();
        InitializeHand();
        InitializeAttacks();

        _attackRoutine = StartCoroutine(BossLoop());
    }

    #region >>> other links

    private void GetOtherLinks()
    {
        _player = FindAnyObjectByType<Player>();
        if (_player == null) { Debug.LogError("Error: Cant Find Player on scene"); return; }

    }

    #endregion
    #region >>> HAND

    private void InitializeHand()
    {
        _hand = FindAnyObjectByType<BossHand>();
        if (_hand == null) { Debug.LogError("Error: Cant Find BossHand on scene"); return; }
        _hand.Initialize(this, _player);
    }

    public void ToggleLookAtPlayer(bool value)
    {
        _hand.ToggleLookAtPlayer(value);
    }

    #endregion
    #region >>> ATTACKS

    private void InitializeAttacks()
    {
        _gunAttack = GetComponent<BossGunAttack>();
        _wallAttack = GetComponent<BossWallAttack>();
        _laserAttack = GetComponent<BossLaserAttack>();
        _gunAttack.Initiazlie(this);
        _laserAttack.Initialize(this);
    }

    private IEnumerator BossLoop()
    {
        while (true)
        {            
            yield return new WaitForSeconds(_idleTime);

            int attack = Random.Range(0, 3);

            switch (attack)
            {
                case 0:
                    yield return _gunAttack.Execute();
                    break;

                case 1:
                    yield return _wallAttack.Execute();
                    break;

                case 2:
                    
                    yield return _laserAttack.Execute();
                    ToggleLookAtPlayer(true);
                    break;
            }
        }

    }
    #endregion
    #region >>> DAMAGE

    public void TakeDamage()
    {
        if (_isDead)
            return;
        if (_takeDamageCoroutine != null)
        {
            StopCoroutine(_takeDamageCoroutine);
            _renderer.materials[0] = _mainMaterial;
        }
        _takeDamageCoroutine = StartCoroutine(TakeDamageVisualRoutine());
        _health--;

        if (_health <= 0)
            Die();
    }

    protected void Die()
    {
        _isDead = true;
        StopCoroutine(_attackRoutine);
        StartCoroutine(DeadRoutine());
    }

    protected virtual void OnTouchPlayer(Player player)
    {
        player.TakeDamage();
        Die();
    }

    protected IEnumerator TakeDamageVisualRoutine()
    {
        Material[] mats = _renderer.materials;

        mats[0] = _takeDamageMaterial;
        _renderer.materials = mats;

        yield return new WaitForSecondsRealtime(0.1f);

        mats[0] = _mainMaterial;
        _renderer.materials = mats;
    }

    protected IEnumerator DeadRoutine()
    {
        Material[] mats = _renderer.materials;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSecondsRealtime(0.2f);

            mats[0] = _takeDamageMaterial;
            _renderer.materials = mats;

            yield return new WaitForSecondsRealtime(0.2f);

            mats[0] = _mainMaterial;
            _renderer.materials = mats;
        }
        Instantiate(_deadEffectPrefab,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
        //_root.OnBossDead(this);
    }

    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerBullet>(out PlayerBullet bullet))
        {
            TakeDamage();
            Destroy(bullet.gameObject);
        }

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            OnTouchPlayer(player);
        }
    }
}
