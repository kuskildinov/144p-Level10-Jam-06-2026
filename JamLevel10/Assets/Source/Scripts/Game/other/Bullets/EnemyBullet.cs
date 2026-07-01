using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifetime = 5f;

    private Vector3 _direction;

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;

        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        transform.position +=
            _direction *
            _speed *
            Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage();
            Destroy(this.gameObject);
        }
    }
}
