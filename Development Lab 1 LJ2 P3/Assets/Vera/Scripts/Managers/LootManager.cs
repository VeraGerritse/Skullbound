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

    public GameObject parent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Loot(Transform spawn, bool chest)
    {
        int tier = TierManager.tier;
        print(tier);
        int r = Random.Range(0, 100);
        if (r < procentalHigher && !TierManager.instance.highestTier || !TierManager.instance.highestTier && chest)
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
            Looting(tier1,spawn, chest);
        }
        if (tier == 2)
        {
            Looting(tier2,spawn, chest);
        }
        if (tier == 3)
        {
            Looting(tier3,spawn,chest);
        }
    }

    public void Looting(List<GameObject> myTier, Transform spawn,bool chest)
    {
        int newItem = Random.Range(0, myTier.Count);
        

        if (chest)
        {

            Vector3 pos = new Vector3(spawn.position.x, spawn.position.y + 1.5f, spawn.position.z);
            Vector3 rot = new Vector3(spawn.rotation.x, spawn.rotation.y, spawn.rotation.z);
            GameObject itemChest = Instantiate(myTier[newItem], pos, spawn.rotation);
            itemChest.transform.Rotate(0, 90, 0);
            itemChest.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GameObject item = Instantiate(myTier[newItem], spawn.position, Quaternion.identity);
        }
    }
}
