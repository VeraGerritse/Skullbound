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


    IEnumerator EndFloor()
    {
        yield return new WaitForSeconds(2);
        DungeonGeneratorManager.instance.Player();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
