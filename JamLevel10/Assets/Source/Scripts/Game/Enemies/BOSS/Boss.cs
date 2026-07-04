using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _idleTime = 5f;
    [SerializeField] private float _health = 80f;
    [Header("Visual")]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _takeDamageMaterial;
    [Header("Dead Effect")]
    [SerializeField] private DeadEffect _deadEffectPrefab;

    private Level_bossFlow _flow;
    private Player _player;
    private BossHand _hand;
    private BossGunAttack _gunAttack;
    private BossWallAttack _wallAttack;
    private BossLaserAttack _laserAttack;

    private bool _isDead;
    private int _attackIndex;
    private Coroutine _attackRoutine;
    private Coroutine _takeDamageCoroutine;

    public Player Player => _player;

    public void Initialzie(Level_bossFlow flow)
    {
        _flow = flow;

        GetOtherLinks();
        InitializeHand();
        InitializeAttacks();
        _attackIndex = 0;
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
        yield return new WaitForSeconds(_idleTime);

        if (_attackIndex > 2)
            _attackIndex = 0;

        while (true)
        {            
            switch (_attackIndex)
            {
                case 0:
                    _flow.ShowBoss();                   
                    yield return _gunAttack.Execute();
                    _attackIndex++;
                    break;

                case 1:
                    _flow.HideBoss();                   
                    yield return _wallAttack.Execute();
                    _attackIndex++;
                    break;

                case 2:
                    _flow.ShowBoss();                   
                    yield return _laserAttack.Execute();                   
                    ToggleLookAtPlayer(true);
                    _attackIndex++;
                    break;
            }
        }       
    }
    #endregion
    #region >>> DAMAGE

    public void TakeDamage(float value)
    {
        if (_isDead)
            return;
        if (_takeDamageCoroutine != null)
        {
            StopCoroutine(_takeDamageCoroutine);
            _renderer.materials[0] = _mainMaterial;
        }
        _takeDamageCoroutine = StartCoroutine(TakeDamageVisualRoutine());
        _health -= value;

        if (_health <= 0)
            Die();
    }

    protected void Die()
    {
        if (_isDead) return;

        _isDead = true;

        StopAllCoroutines();

        _attackRoutine = null;

        StartCoroutine(DeadRoutine());
    }

    protected virtual void OnTouchPlayer(Player player)
    {
        player.TakeDamage();
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
        _flow.OnBossDead();
        Destroy(this.gameObject);        
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerBullet>(out PlayerBullet bullet))
        {
            TakeDamage(GlobalVars.CurrentPlayerDamage);
            bullet.ContactEnemy();
        }

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            OnTouchPlayer(player);
        }
    }
}
