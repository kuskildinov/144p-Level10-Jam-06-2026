using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private HealthPanel _healthPanel;

    private PlayerRoot _root;

    public void Initialize(PlayerRoot root)
    {
        _root = root;
    }

    public void UpdateHealthCount(int healthCount)
    {
        _healthPanel.UpdateHealthCount(healthCount);
    }
}
