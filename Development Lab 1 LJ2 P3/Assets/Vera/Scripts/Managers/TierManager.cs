using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierManager : MonoBehaviour {

    public static TierManager instance;
    public int multiplier;
    public int tier;

    public bool highestTier;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        
    }
}
