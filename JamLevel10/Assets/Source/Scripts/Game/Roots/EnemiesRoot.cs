using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoot : CompositeRoot
{ 
    private EnemiesSpawner _spawner;
    private PlayerRoot _playerRoot;
    private List<Enemy> _enemysOnLevel;
    private EnemiesWave _currentWave;
    private int _index;
    private Coroutine _spawnCoroiutine;
    private bool _spawnFinished;

    public bool AllEnemiesDead => _enemysOnLevel.Count <= 0;
    public bool WaveCompleted => _spawnFinished && _enemysOnLevel.Count == 0;
    public Player Player => _playerRoot.Player;

    public override void Compose()
    {
        _enemysOnLevel = new List<Enemy>();
               
        InitializeSpawner();
        GetOtherLinks();
    }

    private void GetOtherLinks()
    {
        _playerRoot = FindAnyObjectByType<PlayerRoot>();
        if (_playerRoot == null) Debug.LogError("Cant find PlayerRoot on scene for PlayerRoot");
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
        _spawnFinished = false;
        _index = 0;
        SpawnEnemy(_currentWave.Enemies[_index]);
    }

    private void SpawnEnemy(EnemySpawnResponce responce)
    {
        if (_spawnCoroiutine != null)
            StopCoroutine(_spawnCoroiutine);

       _spawnCoroiutine = StartCoroutine(SpawnRoutine(responce));
    }

    private IEnumerator TrySpawnNextGroup()
    {
        _index++;       
        if (_index >= _currentWave.Enemies.Count)
        {
            _index = 0;           
            _spawnFinished = true;
            yield break;
        }
        SpawnEnemy(_currentWave.Enemies[_index]);
    }

    private IEnumerator SpawnRoutine(EnemySpawnResponce responce)
    {
        yield return new WaitForSecondsRealtime(responce.WaitTime);
        _spawner.SpawnEnemiesByType(responce.Type, responce.SpawnPointIndex);
      
        yield return TrySpawnNextGroup();      
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
        _spawner.TrySpawnLoot(enemy.transform.position);
        Destroy(enemy.gameObject);       
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
}
