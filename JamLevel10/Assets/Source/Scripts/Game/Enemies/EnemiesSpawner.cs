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

    private EnemiesRoot _root;

    public void Initialize(EnemiesRoot root)
    {
        _root = root;
    }

    public void SpawnEnemiesByType(EnemyType type, int spawnPointIndex, LootType lootType)
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
        Loot lootPrefab = GetLootPrefabByType(lootType);
        enemy.Initialize(_root, attackPoint, lootPrefab);
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

    private Loot GetLootPrefabByType(LootType type)
    {
        Loot lootPrefab = null;
        switch (type)
        {
            case LootType.None:
                lootPrefab = null;
                break;
            case LootType.Cristal:
                lootPrefab = _coinLootPrefab;
                break;
            case LootType.Health:
                lootPrefab = _aidLootPrefab;
                break;
            default:
                break;
        }
        return lootPrefab;
    }   
}
