using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int _maxHp = 3;
    [Header("Bounds")]
    [SerializeField] protected float _leftDestroyX = -15f;
    [SerializeField] protected float _rightDestroyX = 15f;  
    [SerializeField] protected float _topDestroyY = 10f;
    [SerializeField] protected float _bottomDestroyY = -10f;

    protected int _currentHp;
    protected bool _isDead;

    protected virtual void Awake()
    {
        _currentHp = _maxHp;
    }

    public virtual void TakeDamage()
    {
        if (_isDead)
            return;

        _currentHp --;

        if (_currentHp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        _isDead = true;
        Destroy(gameObject);
    }

    protected virtual void OnTouchPlayer(Player player)
    {
        player.TakeDamage();
        Die();
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