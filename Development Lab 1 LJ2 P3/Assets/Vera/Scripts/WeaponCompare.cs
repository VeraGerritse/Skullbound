using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCompare : MonoBehaviour {
    public static WeaponCompare instance;
    public List<Text> statText = new List<Text>();
    public List<Text> statNumbers = new List<Text>();
    public Text nameText;
    public Canvas canvasDiff;

    public List<string> weaponStats = new List<string>();
    public List<string> shieldStats = new List<string>();


    private void Awake()
    {
        instance = this;
    }

    public void CalDiffShield(Shield toCompare)
    {
        List<string> diff = new List<string>();
        List<float> numberDiff = new List<float>();
        List<bool> lowerOrHigher = new List<bool>();

        Shield inHand = PlayerStats.instance.shield.GetComponent<Shield>();
        if (toCompare.stability < inHand.stability)
        {
            diff.Add(shieldStats[0]);
            numberDiff.Add(toCompare.stability - inHand.stability);
            lowerOrHigher.Add(false);
        }
        else if (toCompare.stability > inHand.stability)
        {
            diff.Add(shieldStats[0]);
            numberDiff.Add(toCompare.stability - inHand.stability);
            lowerOrHigher.Add(true);
        }

        if (toCompare.bashcost < inHand.bashcost)
        {
            diff.Add(shieldStats[1]);
            numberDiff.Add(toCompare.bashcost - inHand.bashcost);
            lowerOrHigher.Add(true);
        }
        else if (toCompare.bashcost > inHand.bashcost)
        {
            diff.Add(shieldStats[1]);
            numberDiff.Add(toCompare.bashcost - inHand.bashcost);
            lowerOrHigher.Add(false);
        }

        if (toCompare.bashDamage < inHand.bashDamage)
        {
            diff.Add(shieldStats[2]);
            numberDiff.Add(toCompare.bashDamage - inHand.bashDamage);
            lowerOrHigher.Add(false);
        }
        else if (toCompare.bashDamage > inHand.bashDamage)
        {
            diff.Add(shieldStats[2]);
            numberDiff.Add(toCompare.bashDamage - inHand.bashDamage);
            lowerOrHigher.Add(true);
        }
        DisplayDifference(diff, numberDiff, lowerOrHigher,toCompare.shieldName);
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
            numberDiff.Add(toCompare.staminacost - inHand.staminacost);
            lowerOrHigher.Add(true);
        }
        if (toCompare.staminacost > inHand.staminacost)
        {
            diff.Add(weaponStats[1]);
            numberDiff.Add(toCompare.staminacost - inHand.staminacost);
            lowerOrHigher.Add(false);
        }
        DisplayDifference(diff, numberDiff, lowerOrHigher, toCompare.weaponName);
    }

    void DisplayDifference(List<string> what, List<float> howMuch, List<bool> lowerOrHigher, string name)
    {
            nameText.text = name;
            canvasDiff.enabled = true;
        for (int i = 0; i < statText.Count; i++)
        {
            if(i < what.Count)
            {
                statText[i].enabled = true;
                statNumbers[i].enabled = true;
                statText[i].text = what[i];
                if(howMuch[i] > 0)
                {
                    statNumbers[i].text = "+" + howMuch[i].ToString();
                }
                else
                {
                    statNumbers[i].text = howMuch[i].ToString();
                }
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

    public void CloseCanvas()
    {
        canvasDiff.enabled = false;
    }
}
