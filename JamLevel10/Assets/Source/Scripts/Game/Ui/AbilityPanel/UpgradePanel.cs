using DG.Tweening;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeRoot _root;
    [SerializeField] private Player player;
    [SerializeField] private AbilityIcon[] buttons;
    [Header("Anim")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panelRoot;

    public void Open()
    {
        _panel.gameObject.SetActive(true);

        var upgrades = _root.GetRandomUpgrades(3);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Setup(upgrades[i], this);
        }
                
        canvasGroup.alpha = 0f;
        panelRoot.localScale = Vector3.one * 0.8f;

        Time.timeScale = 0f;
               
        canvasGroup.DOFade(1f, 0.25f)
            .SetUpdate(true);

        panelRoot.DOScale(1f, 0.25f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        SoundsRoot.Instance.PlayClickSound();
        player.ApplyUpgrade(upgrade);

        Close();
    }

    private void Close()
    {
        canvasGroup.DOFade(0f, 0.2f).SetUpdate(true);
        panelRoot.DOScale(0.85f, 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _panel.gameObject.SetActive(false);
                Time.timeScale = 1f;
                _root.OnPanelClosed();
            });
    }
}