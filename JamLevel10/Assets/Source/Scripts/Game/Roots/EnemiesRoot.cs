using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoot : CompositeRoot
{
    [SerializeField] private SpawnEnemyData _testData;
    [SerializeField] private Boss _boss;
    private EnemiesSpawner _spawner;

    private List<Enemy> _enemysOnLevel;
    private EnemiesWave _currentWave;
    private int _index;
    private Coroutine _spawnCoroiutine;

    public override void Compose()
    {
        _enemysOnLevel = new List<Enemy>();

        if (_boss != null)
            _boss.Initialzie();
        InitializeSpawner();
    }

    private void Start()
    {
        StartSpawnWay(_testData.Responce);
    }

    #region >>> SPAWNER

    private void InitializeSpawner()
    {
        _spawner = FindAnyObjectByType<EnemiesSpawner>();
        if (_spawner == null) { Debug.LogError("Error: Cant find EnemiesSpawner on scene"); return; }
        _spawner.Initialize(this);
    }

    public void StartSpawnWay(EnemiesWave wave)
    {
        if (wave == null || wave.Enemies == null || wave.Enemies.Count <= 0)
            return;

        _currentWave = wave;
        _index = 0;
        SpawnEnemy(_currentWave.Enemies[_index]);
    }

    public void SpawnEnemy(EnemySpawnResponce responce)
    {
        if (_spawnCoroiutine != null)
            StopCoroutine(_spawnCoroiutine);

        _spawnCoroiutine = StartCoroutine(SpawnRoutine(responce));
    }

    private void TrySpawnNextGroup()
    {
        _index++;
        if(_index >= _currentWave.Enemies.Count)
        {
            _index = 0;
            return;
        }
        SpawnEnemy(_currentWave.Enemies[_index]);
    }

    private IEnumerator SpawnRoutine(EnemySpawnResponce responce)
    {
        yield return new WaitForSecondsRealtime(responce.WaitTime);
        _spawner.SpawnEnemiesByType(responce.Type, responce.SpawnPointIndex, responce.Loot);       
        TrySpawnNextGroup();      
    }
    #endregion
    #region >>> ENEMIES BEHAVIOUYR

    public void OnEnemySpawned(Enemy enemy)
    {
        _enemysOnLevel.Add(enemy);
    }

    public void OnEnemyDead(Enemy enemy)
    {      
        _enemysOnLevel.Remove(enemy);
        Destroy(enemy.gameObject);
        CheckAllEnemiesDead();
    }

    private void CheckAllEnemiesDead()
    {
        if(_enemysOnLevel.Count <= 0)
        {
            StartSpawnWay(_testData.Responce);
        }
    }

    #endregion
}

[Serializable]
public class EnemiesWave
{
    public List<EnemySpawnResponce> Enemies;
}

[Serializable]
public class EnemySpawnResponce
{
    public EnemyType Type;
    public int SpawnPointIndex;
    public float WaitTime;
    public LootType Loot;
}
