using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TierManager : MonoBehaviour {

    public static TierManager instance;
    public int multiplier;
    public static int tier = 1;
    public int highestTiers;
    public bool highestTier;

    public int procentalHigher;
    public int procentalLower;
    [Header("TieredSkelletons")]
    public List<GameObject> skellyTier1 = new List<GameObject>();
    public List<GameObject> skellyTier2 = new List<GameObject>();
    public List<GameObject> skellyTier3 = new List<GameObject>();

    [Header("Bosses")]
    public List<GameObject> bossTiers = new List<GameObject>();



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void HigherTier()
    {
        if(tier < highestTiers)
        {
            tier++;
            if(tier == highestTiers)
            {
                highestTier = true;
            }
        }
        StartCoroutine(EndFloor());
    }

    public GameObject RandomSkelleton(bool isBoss)
    {
        if (isBoss)
        {
            return GetBoss();
        }
        int tier = TierManager.tier;
        
        int r = Random.Range(0, 100);
        if (r < procentalHigher && !TierManager.instance.highestTier)
        {
            tier++;
        }
        else
        {
            tier = Random.Range(1, tier + 1);
        }
        if (tier == 1)
        {
            return Skelly(skellyTier1);
        }
        if (tier == 2)
        {
            return Skelly(skellyTier2);
        }
        if (tier == 3)
        {
            return  Skelly(skellyTier3);
        }
        return Skelly(skellyTier1);
    }

    public GameObject Skelly(List<GameObject> myTier)
    {
        int rand = Random.Range(0, myTier.Count);
        return myTier[rand]; 
    }
         
    public GameObject GetBoss()
    {
        if(tier == 1)
        {
            return bossTiers[0];
        }
        if (tier == 2)
        {
            return bossTiers[1];
        }
        if (tier == 3)
        {
            return bossTiers[2];
        }
        return bossTiers[0];
    }
    IEnumerator EndFloor()
    {
        yield return new WaitForSeconds(2);
        DungeonGeneratorManager.instance.Player();
        PlayerStats.instance.SaveWeapons();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
