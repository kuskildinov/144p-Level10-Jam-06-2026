using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level_bossFlow : MonoBehaviour,ILevelFlow
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _hidePoint;
    [SerializeField] private List<SpawnEnemyData> _enemysData;

    private LevelRoot _root;
    private EnemiesRoot _enemiesRoot;
    private Tween _bossMoveTween;
    private Coroutine _flowCoroutine;

    public void Initialzie(LevelRoot root)
    {
        _root = root;
        _enemiesRoot = FindAnyObjectByType<EnemiesRoot>();

        _boss.transform.position = _hidePoint.position;
        StartNewFlow();
    }

    public void StartNewFlow()
    {
        SoundsRoot.Instance.PlayBossPhrase();
        ShowDialogByIndex(7, () =>
        {
            _root.HideDialog();
            if (_flowCoroutine != null)
                StopCoroutine(_flowCoroutine);
            _flowCoroutine = StartCoroutine(StartFlow());
        });
    }

    private void ShowDialogByIndex(int index, Action onComplete = null)
    {
        _root.ShowDialogByIndex(index, () =>
        {
            _root.HideDialog();
            onComplete?.Invoke();
        });
    }

    public void ShowBoss()
    {
        _boss.gameObject.SetActive(true);
        _bossMoveTween = _boss.transform.DOMove(_attackPoint.position, 3)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                
            });
    }

    public void HideBoss()
    {
        _boss.gameObject.SetActive(true);
        _bossMoveTween = _boss.transform.DOMove(_hidePoint.position, 3)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                
            });
    }

    public void OnBossDead()
    {
        _root.StartEndCutScene();
    }

    public IEnumerator StartFlow()
    {
        SoundsRoot.Instance.StartBossSound();
        ShowBoss();
        _boss.Initialzie(this);
        ShowDialogByIndex(7);
        yield return null;
    }

    private void OnDisable()
    {
        if (_flowCoroutine != null)
            StopCoroutine(_flowCoroutine);

    }
}
