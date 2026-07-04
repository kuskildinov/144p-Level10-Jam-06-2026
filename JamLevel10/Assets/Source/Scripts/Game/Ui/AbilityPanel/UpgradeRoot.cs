using System.Collections.Generic;
using UnityEngine;

public class UpgradeRoot : CompositeRoot
{
    [SerializeField] private List<Upgrade> allUpgrades;

    public override void Compose()
    {
       
    }

    public List<Upgrade> GetRandomUpgrades(int count = 3)
    {
        List<Upgrade> result = new List<Upgrade>();

        List<Upgrade> pool = new List<Upgrade>(allUpgrades);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    public void OnPanelClosed()
    {
        GlobalVars.CristalCount = 0;
        GlobalVars.CurrentMaxCristalCount += 3;

        PlayerRoot playerRoot = FindAnyObjectByType<PlayerRoot>();
        playerRoot.UpdateCristalCount();       
    }
}