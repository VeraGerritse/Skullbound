using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUI : MonoBehaviour {
    public List<Image> potions = new List<Image>();
    public List<Image> syringe = new List<Image>();
    public static PotionUI instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void UpdatePotions(int amount)
    {
        for (int i = 0; i < potions.Count; i++)
        {
            if(i < amount)
            {
                potions[i].enabled = true;
            }
            else
            {
                potions[i].enabled = false;
            }
        }
    }

    public void UpdateSyringe(int amount)
    {
        for (int i = 0; i < syringe.Count; i++)
        {
            if (i < amount)
            {
                syringe[i].enabled = true;
            }
            else
            {
                syringe[i].enabled = false;
            }
        }
    }
}
