using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] protected float _speed = 15f;
    [SerializeField] protected float _lifeTime = 5f;
    [SerializeField] protected int _damage = 1;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }
}
