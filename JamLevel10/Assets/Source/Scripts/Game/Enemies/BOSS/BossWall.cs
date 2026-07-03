using UnityEngine;

public class BossWall : MonoBehaviour
{
    private float _speed;

    public void Init(float speed)
    {
        _speed = speed;
    }

    private void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent < Player>(out Player player))
        {
            player.TakeDamage();
        }
    }
}
