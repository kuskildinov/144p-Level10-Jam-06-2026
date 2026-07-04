using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private HealthPanel _healthPanel;
    [SerializeField] private CristalPanel _cristalPanel;   

    private PlayerRoot _root;

    public void Initialize(PlayerRoot root)
    {
        _root = root;
    }

    public void UpdateHealthCount(int healthCount)
    {
        _healthPanel.UpdateHealthCount(healthCount);
    }

    public void ToggleVisibility(bool value)
    {
        _healthPanel.gameObject.SetActive(value);
        _cristalPanel.gameObject.SetActive(value);
    }

    public void UpdateCristalCount()
    {
        _cristalPanel.UpdateCristalCount();
    }

    public void SetMaxCristalCount()
    {
        _cristalPanel.SetMaxCristalCount();
    }
}
