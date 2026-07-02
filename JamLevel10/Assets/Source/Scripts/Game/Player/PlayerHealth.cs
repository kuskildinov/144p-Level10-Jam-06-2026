using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Player _player;
    private int _maxHp = 4;
    private int _takedDamageCount;

    public int CurrentHP;

    public void Initialize(Player player)
    {
        _player = player;
        UpdateCurrentHP();
    }    

    public void OnDamageTaked()
    {
        _takedDamageCount++;
        if (_takedDamageCount >= _maxHp)
            _takedDamageCount = _maxHp;

        UpdateCurrentHP();
    }

    public void OnHealthTaked()
    {
        _takedDamageCount--;
        if (_takedDamageCount <= 0)
            _takedDamageCount = 0;

        UpdateCurrentHP();
    }

    private void UpdateCurrentHP()
    {
        CurrentHP = _maxHp - _takedDamageCount;
        if (CurrentHP <= 0)
            _player.Dead();
    }
}
