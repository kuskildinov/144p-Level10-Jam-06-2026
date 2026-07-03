using UnityEngine;

public class BossHand : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    private Player _player;
    private Boss _boss;

    private bool _lookAtPlayer = true;

    public void Initialize(Boss boss, Player player)
    {
        _player = player;
        _boss = boss;
    }

    void Update()
    {
        if (!_lookAtPlayer || _player == null)
            return;

        LookAtPlayerHanlder();
    }

    public void ToggleLookAtPlayer(bool value)
    {
        _lookAtPlayer = value;
    }

    private void LookAtPlayerHanlder()
    {
        Vector3 direction = _player.transform.position - transform.position;

        // убираем ось глубины (Z), если игра боковая
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
