using DG.Tweening;
using UnityEngine;

public class HeartIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _rect;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Animation")]
    [SerializeField] private float _showDuration = 0.25f;
    [SerializeField] private float _hideDuration = 0.25f;
    [SerializeField] private float _breathScale = 1.03f;
    [SerializeField] private float _breathDuration = 0.8f;

    private Vector2 _startPosition;

    private Sequence _showSequence;
    private Sequence _hideSequence;
    private Tween _breathTween;

    public bool IsShow;

    private void Start()
    {
        _startPosition = _rect.anchoredPosition;
    }

    private void OnDisable()
    {
        _showSequence?.Kill();
        _hideSequence?.Kill();
        _breathTween?.Kill();
    }

    public void Show()
    {
        if (IsShow)
            return;

        IsShow = true;
        _showSequence?.Kill();
        _hideSequence?.Kill();
        _breathTween?.Kill();

        _rect.localScale = Vector3.zero;
        _rect.anchoredPosition = _startPosition + Vector2.down * 25f;
        _canvasGroup.alpha = 0f;

        _showSequence = DOTween.Sequence();

        _showSequence.Join(
            _rect.DOScale(1.15f, _showDuration)
                .SetEase(Ease.OutBack));

        _showSequence.Join(
            _rect.DOAnchorPos(_startPosition, _showDuration)
                .SetEase(Ease.OutCubic));

        _showSequence.Join(
            _canvasGroup.DOFade(1f, _showDuration));

        _showSequence.Append(
            _rect.DOScale(1f, 0.12f)
                .SetEase(Ease.OutQuad));

        _showSequence.OnComplete(StartBreathing);
    }

    public void Hide()
    {
        if (!IsShow)
            return;

        IsShow = false;
        _showSequence?.Kill();
        _hideSequence?.Kill();
        _breathTween?.Kill();

        _hideSequence = DOTween.Sequence();

        _hideSequence.Append(
            _rect.DOScale(1.1f, 0.08f));

        _hideSequence.Join(
            _canvasGroup.DOFade(0f, _hideDuration));

        _hideSequence.Join(
            _rect.DOAnchorPos(_startPosition + Vector2.up * 25f, _hideDuration)
                .SetEase(Ease.InCubic));

        _hideSequence.Append(
            _rect.DOScale(0f, 0.15f)
                .SetEase(Ease.InBack));
    }

    private void StartBreathing()
    {
        _breathTween?.Kill();

        _breathTween = _rect
            .DOScale(_breathScale, _breathDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
