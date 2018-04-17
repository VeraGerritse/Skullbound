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

    [Header("TieredSkelletons")]
    List<GameObject> skellyTier1 = new List<GameObject>();
    List<GameObject> skellyTier2 = new List<GameObject>();
    List<GameObject> skellyTier3 = new List<GameObject>();

    public int procentalHigher;
    public int procentalLower;

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

    public GameObject RandomSkelleton()
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
         

    IEnumerator EndFloor()
    {
        yield return new WaitForSeconds(2);
        DungeonGeneratorManager.instance.Player();
        PlayerStats.instance.SaveWeapons();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
