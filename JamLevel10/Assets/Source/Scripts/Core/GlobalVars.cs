using UnityEngine;

public static class GlobalVars
{
    public static int CristalCount = 0;
    public static int CurrentMaxCristalCount = 1;
    public static bool NeedSkipTips = true;
    [Header("Abilities")]
    public static float StartPlayerDamage = 1;
    public static float CurrentPlayerDamage = 1;

    public static float StartPlayerFiraRate = 0.3f;
    public static float CurrentPlayerFiraRate = 0.3f;

    public static int StartPlayerBulletCount = 1;
    public static int CurrentPlayerBulletCount = 1;

    public static float StartPlayerBulletSize = 0;
    public static float CurrentPlayerBulletSize = 0;
    
    public static int StartPlayerBulletThrow = 1;
    public static int CurrentPlayerBulletThrow = 1;

    public static int StartPlayerExpMultiply = 1;
    public static int CurrentPlayerExpMultiply = 1;

    public static int Level = 1;
}
