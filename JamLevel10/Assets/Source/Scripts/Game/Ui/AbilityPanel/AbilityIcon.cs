using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button button;

    private Upgrade upgrade;
    private UpgradePanel panel;

    public void Setup(Upgrade upgrade, UpgradePanel panel)
    {
        this.upgrade = upgrade;
        this.panel = panel;

        _image.sprite = upgrade.Icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => panel.SelectUpgrade(upgrade));
    }
}
