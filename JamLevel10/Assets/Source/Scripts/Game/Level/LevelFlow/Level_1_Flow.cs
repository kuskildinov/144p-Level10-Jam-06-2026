using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level_1_Flow : MonoBehaviour, ILevelFlow
{
    [SerializeField] private List<SpawnEnemyData> _enemysData;

    private LevelRoot _root;
    private EnemiesRoot _enemiesRoot;
    private Coroutine _flowCoroutine;

    public void Initialzie(LevelRoot root)
    {
        _root = root;
        _enemiesRoot = FindAnyObjectByType<EnemiesRoot>();

    }

    private void Start()
    {
        if(GlobalVars.NeedSkipTips)
        {
            if (_flowCoroutine != null)
                StopCoroutine(_flowCoroutine);
            _flowCoroutine = StartCoroutine(StartFlow());
        }
        else
        {
            if (_flowCoroutine != null)
                StopCoroutine(_flowCoroutine);
            _flowCoroutine = StartCoroutine(StartTipsFlow());
        }       
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

        while(true)
        {
            int randIndex = Random.Range(0, _enemysData.Count);
            _enemiesRoot.StartSpawnWay(_enemysData[randIndex].Responce);
            yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);
        }
    }

    public IEnumerator StartTipsFlow()
    {
        yield return new WaitForSecondsRealtime(3f);      
        ShowDialogByIndex(0,() =>
        {           
            ShowDialogByIndex(1);           
        });
        yield return new WaitForSecondsRealtime(20f);

        _enemiesRoot.StartSpawnWay(_enemysData[0].Responce);
        yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);

        _enemiesRoot.StartSpawnWay(_enemysData[1].Responce);        
        yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);
       
        _enemiesRoot.StartSpawnWay(_enemysData[2].Responce);
        yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);

        _enemiesRoot.StartSpawnWay(_enemysData[3].Responce);
        yield return new WaitUntil(() => _enemiesRoot.WaveCompleted);

        if (_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);
        _flowCoroutine = StartCoroutine(StartFlow());

        yield break;
    }

    private void OnDisable()
    {
        StopCoroutine(_flowCoroutine);
    }
}
