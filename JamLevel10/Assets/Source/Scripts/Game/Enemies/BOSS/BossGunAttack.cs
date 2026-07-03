using System.Collections;
using UnityEngine;

public class BossGunAttack : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnemyBullet _bulletPrefab;
    [SerializeField] private Transform _firePoint;

    [SerializeField] private float _shootDuration = 3f;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private float _pauseAfter = 1f;

    private Boss _boss;

    public void Initiazlie(Boss boss)
    {
        _boss = boss;
    }

    public IEnumerator Execute()
    {
        float timer = 0f;

        while (timer < _shootDuration)
        {
            Shoot();
            yield return new WaitForSeconds(_fireRate);
            timer += _fireRate;
        }

        yield return new WaitForSeconds(_pauseAfter);
    }

    private void Shoot()
    {
        if (_boss == null || _boss.Player == null)
            return;

        EnemyBullet bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        Vector2 direction =
             (_boss.Player.transform.position -
              _firePoint.position).normalized;
        bullet.Initialize(direction);
    }
}
