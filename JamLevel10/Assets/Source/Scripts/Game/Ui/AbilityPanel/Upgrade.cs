using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    public Sprite Icon;
    public UpgradeType type;
}

public enum UpgradeType
{
    Damage,
    FireRate,
    BulletCount,
    ThropwForce,
    BulletSize,
    Exp,
}