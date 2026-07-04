using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [Header("Spawn Points")]
    [SerializeField] private Transform _topSpawnPoint;
    [SerializeField] private Transform _centerTopSpawnPoint;
    [SerializeField] private Transform _centerBottomSpawnPoint;
    [SerializeField] private Transform _bottomSpawnPoint;
    [Header("Attack Points")]
    [SerializeField] private Transform _topAttackPoint;
    [SerializeField] private Transform _centerTopAttackPoint;
    [SerializeField] private Transform _centerBottomAttackPoint;
    [SerializeField] private Transform _bottomAttackPoint;
    [Header("Enemys Prefabs")]
    [SerializeField] private Enemy _enemy_1_prefab;
    [SerializeField] private Enemy _enemy_2_prefab;
    [SerializeField] private Enemy _enemy_3_prefab;
    [Header("Loots Prefabs")]
    [SerializeField] private Loot _aidLootPrefab;
    [SerializeField] private Loot _coinLootPrefab;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;
    [SerializeField]
    private float[] _healthDropChance =
{
    0.45f, // HP = 0
    0.45f, // HP = 1
    0.30f, // HP = 2
    0.15f, // HP = 3
    0.00f  // HP = 4
};

    private EnemiesRoot _root;

    public void Initialize(EnemiesRoot root)
    {
        _root = root;
    }

    public void SpawnEnemiesByType(EnemyType type, int spawnPointIndex)
    {
        Enemy currentEnemyPrefab = null;
        Transform spawnPoint = GetSpawnPointByIndex(spawnPointIndex);

        switch (type)
        {
            case EnemyType.Macrophage:
                {
                    currentEnemyPrefab = _enemy_1_prefab;
                    break;
                }

            case EnemyType.Kamikadze:
                {
                    currentEnemyPrefab = _enemy_2_prefab;
                    break;
                }
            case EnemyType.Turret:
                {
                    currentEnemyPrefab = _enemy_3_prefab;
                    break;
                }
            default:
                break;
        }

        if (currentEnemyPrefab == null)
            return;

        Transform attackPoint = GetAttackPointByIndex(spawnPointIndex);
        Enemy enemy = Instantiate(currentEnemyPrefab,spawnPoint.position,Quaternion.identity);
        enemy.Initialize(_root, attackPoint);
        _root.OnEnemySpawned(enemy);
    }

    private Transform GetSpawnPointByIndex(int index)
    {
        Transform spawnPoint = null;
        switch (index)
        {
            case 0:
                {
                    spawnPoint = _topSpawnPoint;
                    break;
                }
            case 1:
                {
                    spawnPoint = _centerTopSpawnPoint;
                    break;
                }
            case 2:
                {
                    spawnPoint = _centerBottomSpawnPoint;
                    break;
                }
            case 3:
                {
                    spawnPoint = _bottomSpawnPoint;
                    break;
                }
            default:
                {
                    spawnPoint = _topSpawnPoint;
                    break;
                }
        }

        return spawnPoint;
    }

    private Transform GetAttackPointByIndex(int index)
    {
        Transform attackPoint = null;
        switch (index)
        {
            case 0:
                {
                    attackPoint = _topAttackPoint;
                    break;
                }
            case 1:
                {
                    attackPoint = _centerTopAttackPoint;
                    break;
                }
            case 2:
                {
                    attackPoint = _centerBottomAttackPoint;
                    break;
                }
            case 3:
                {
                    attackPoint = _bottomAttackPoint;
                    break;
                }
            default:
                {
                    attackPoint = _topAttackPoint;
                    break;
                }
        }

        return attackPoint;
    }

    public void TrySpawnLoot(Vector3 position)
    {
        if (!IsInsideBounds(position))
            return;

        int hp = Mathf.Clamp(_root.Player.CurrentHP, 0, _healthDropChance.Length - 1);

        if (Random.value <= _healthDropChance[hp])
        {
            Instantiate(_aidLootPrefab, position,Quaternion.identity);
        }
        else
        {
            Instantiate(_coinLootPrefab, position, Quaternion.identity);
        }
    }

    private bool IsInsideBounds(Vector3 position)
    {
        return position.x >= _minBounds.x &&
               position.x <= _maxBounds.x &&
               position.y >= _minBounds.y &&
               position.y <= _maxBounds.y;
    }

}
