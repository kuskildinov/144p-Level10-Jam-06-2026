using UnityEngine;

public class HealthPanel : MonoBehaviour
{
    [SerializeField] private HeartIcon[] _hearts;

    public void UpdateHealthCount(int healthCount)
    {
        healthCount = Mathf.Clamp(healthCount, 0, _hearts.Length);

        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < healthCount)
            {
                _hearts[i].Show();
            }
            else
            {
                _hearts[i].Hide();
            }
        }
    }
}
