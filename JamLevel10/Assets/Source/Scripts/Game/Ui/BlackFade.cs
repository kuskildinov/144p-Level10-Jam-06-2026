using DG.Tweening;
using System;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _rect;

    [Header("Settings")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Ease _easeIn = Ease.OutCubic;
    [SerializeField] private Ease _easeOut = Ease.InCubic;

    private Tween _tween;
       
    public void FadeIn(Action onComplete = null)
    {
        _tween?.Kill();

        gameObject.SetActive(true);

        // старт: точка
        _rect.localScale = Vector3.zero;

        _tween = _rect
            .DOScale(35f, _duration)
            .SetEase(_easeIn)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void FadeOut(Action onComplete = null)
    {
        _tween?.Kill();

        // гарантируем, что экран полностью закрыт
        _rect.localScale = Vector3.one * 35f;

        _tween = _rect
            .DOScale(0f, _duration)
            .SetEase(_easeOut)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }

    public void SetInstantBlack()
    {
        _tween?.Kill();
        gameObject.SetActive(true);
        _rect.localScale = Vector3.one * 35f;
    }

    public void SetInstantClear()
    {
        _tween?.Kill();
        _rect.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}
