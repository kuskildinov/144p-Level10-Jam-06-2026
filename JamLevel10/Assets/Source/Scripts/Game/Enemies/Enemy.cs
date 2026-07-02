using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType _type;
    [SerializeField] protected int _maxHp = 3;
    [Header("Bounds")]
    [SerializeField] protected float _leftDestroyX = -15f;
    [SerializeField] protected float _rightDestroyX = 15f;  
    [SerializeField] protected float _topDestroyY = 10f;
    [SerializeField] protected float _bottomDestroyY = -10f;
    [Header("TakeDamage")]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _takeDamageMaterial;
    [Header("Dead Effect")]
    [SerializeField] private DeadEffect _deadEffectPrefab;

    protected EnemiesRoot _root;
    protected int _currentHp;
    protected bool _isDead;
    protected Coroutine _takeDamageCoroutine;
    protected Loot _currentLootPrefab;

    public EnemyType Type => _type;

    public virtual void Initialize(EnemiesRoot root, Transform attackPoint, Loot lootPrefab)
    {
        _root = root;
        _currentHp = _maxHp;
        _currentLootPrefab = lootPrefab;
    }

    public virtual void TakeDamage()
    {
        if (_isDead)
            return;
        if (_takeDamageCoroutine != null)
        {
            StopCoroutine(_takeDamageCoroutine);
            _renderer.materials[0] = _mainMaterial;
        }
        _takeDamageCoroutine =  StartCoroutine(TakeDamageVisualRoutine());
        _currentHp --;

        if (_currentHp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        _isDead = true;
        Instantiate(_deadEffectPrefab,transform.position,Quaternion.identity);
        TrySpawnLoot();
        _root.OnEnemyDead(this);       
    }

    protected virtual void OnTouchPlayer(Player player)
    {
        player.TakeDamage();
        Die();
    }

    private void TrySpawnLoot()
    {
        if (_currentLootPrefab == null)
            return;
        Instantiate(_currentLootPrefab, transform.position,Quaternion.identity);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerBullet>(out PlayerBullet bullet))
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

public enum EnemyType
{
    Macrophage,
    Kamikadze,
    Turret,
}
