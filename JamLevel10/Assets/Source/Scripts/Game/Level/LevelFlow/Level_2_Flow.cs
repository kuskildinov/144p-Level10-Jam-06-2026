using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level_2_Flow : MonoBehaviour, ILevelFlow
{
    [SerializeField] private List<SpawnEnemyData> _enemysData;

    private LevelRoot _root;
    private EnemiesRoot _enemiesRoot;
    private Coroutine _flowCoroutine;

    public void Initialzie(LevelRoot root)
    {
        _root = root;
        _enemiesRoot = FindAnyObjectByType<EnemiesRoot>();
        StartNewFlow();
    }

    public void StartNewFlow()
    {
        if (_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);
        _flowCoroutine = StartCoroutine(StartFlow());
    }

    private void ShowDialogByIndex(int index, Action onComplete = null)
    {
        _root.ShowDialogByIndex(index, () =>
        {
            _root.HideDialog();
            onComplete?.Invoke();
        });
    }

    public IEnumerator StartFlow()
    {
        yield return new WaitForSecondsRealtime(3f);
        _root.HideDialog();

        while (true)
        {
            int randIndex = Random.Range(0, _enemysData.Count);
            _enemiesRoot.StartSpawnWay(_enemysData[randIndex].Responce);
            yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);
        }
    }

    private void OnDisable()
    {
        if(_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);
    }
}
