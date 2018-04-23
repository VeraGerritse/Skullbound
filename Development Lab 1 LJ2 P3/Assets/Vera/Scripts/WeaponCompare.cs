using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCompare : MonoBehaviour {
    public static WeaponCompare instance;
    public List<Text> statText = new List<Text>();
    public List<Text> statNumbers = new List<Text>();
    public Canvas canvasDiff;

    public List<string> weaponStats = new List<string>();
    public List<string> shieldStats = new List<string>();


    private void Awake()
    {
        instance = this;
    }

    public void CalDiffShield(Shield toCompare)
    {
        //if()
    }
    public void CalDiffWeapon(Weapon toCompare)
    {
        List<string> diff = new List<string>();
        List<float> numberDiff = new List<float>();
        List<bool> lowerOrHigher = new List<bool>();

        Weapon inHand = PlayerStats.instance.weapon.GetComponent<Weapon>();
        if (toCompare.attack < inHand.attack)
        {
            diff.Add(weaponStats[0]);
            numberDiff.Add(toCompare.attack - inHand.attack);
            lowerOrHigher.Add(false);
        }
        else if (toCompare.attack > inHand.attack)
        {
            diff.Add(weaponStats[0]);
            numberDiff.Add(toCompare.attack - inHand.attack);
            lowerOrHigher.Add(true);
        }

        if(toCompare.staminacost < inHand.staminacost)
        {
            diff.Add(weaponStats[1]);
            numberDiff.Add(inHand.staminacost - toCompare.staminacost);
            lowerOrHigher.Add(false);
        }
        if (toCompare.staminacost > inHand.staminacost)
        {
            diff.Add(weaponStats[1]);
            numberDiff.Add(inHand.staminacost - toCompare.staminacost);
            lowerOrHigher.Add(true);
        }
        DisplayDifference(diff, numberDiff, lowerOrHigher);
    }

    void DisplayDifference(List<string> what, List<float> howMuch, List<bool> lowerOrHigher)
    {
        if(what.Count == 0)
        {
            canvasDiff.enabled = false;
            return;
        }
        else
        {
            canvasDiff.enabled = true;
        }
        for (int i = 0; i < statText.Count; i++)
        {
            if(i < what.Count)
            {
                statText[i].enabled = true;
                statNumbers[i].enabled = true;
                statText[i].text = what[i];
                statNumbers[i].text = howMuch[i].ToString();
                if (lowerOrHigher[i])
                {
                    statNumbers[i].color = Color.green;
                }
                else
                {
                    statNumbers[i].color = Color.red;
                }
            }
            else
            {
                statText[i].enabled = false;
                statNumbers[i].enabled = false;
            }

        }
    }
}
