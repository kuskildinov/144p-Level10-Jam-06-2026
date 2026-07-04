using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] protected float _speed = 15f;
    [SerializeField] protected float _lifeTime = 5f;
    [SerializeField] protected int _damage = 1;

    private int _health;
    private Vector3 _direction;

    public void Init(Vector3 direction)
    {
        _direction = direction.normalized;
        _health = GlobalVars.CurrentPlayerBulletThrow;
    }

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void ContactEnemy()
    {
        _health--;
        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<BulletWall>(out BulletWall wall))
        {
            Destroy(gameObject);
        }
    }
}
