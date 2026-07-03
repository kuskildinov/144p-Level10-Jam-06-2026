using System.Collections;
using UnityEngine;

public class BossWallAttack : MonoBehaviour
{
    [SerializeField] private BossWall _wallPrefab;
    [SerializeField] private Transform[] _spawnPoint;

    [SerializeField] private float _spawnCount = 3;
    [SerializeField] private float _spawnDelay = 0.6f;
    [SerializeField] private float _speed = 5f;

    public IEnumerator Execute()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            SpawnWall();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void SpawnWall()
    {
        Transform currentSpawnPoint = _spawnPoint[Random.Range(0, _spawnPoint.Length)];
        BossWall wall = Instantiate(_wallPrefab, currentSpawnPoint.position, Quaternion.identity);

        wall.Init(_speed);
    }
}
