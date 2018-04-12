using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    public List<GameObject> tier1 = new List<GameObject>();
    public List<GameObject> tier2 = new List<GameObject>();
    public List<GameObject> tier3 = new List<GameObject>();

    public int procentalHigher;
    public int procentalLower;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Loot(Transform spawn)
    {
        int tier = TierManager.tier;
        print(tier);
        int r = Random.Range(0, 100);
        if (r < procentalHigher && !TierManager.instance.highestTier)
        {
            tier++;
        }
        else
        {
            tier = Random.Range(1, tier + 1);
        }
        print(tier + "ugh");
        if(tier == 1)
        {
            Looting(tier1,spawn);
        }
        if (tier == 2)
        {
            Looting(tier2,spawn);
        }
        if (tier == 3)
        {
            Looting(tier3,spawn);
        }
    }

    public void Looting(List<GameObject> myTier, Transform spawn)
    {
        int newItem = Random.Range(0, myTier.Count);
        GameObject item = Instantiate(myTier[newItem], spawn.position, Quaternion.identity);
    }
}
