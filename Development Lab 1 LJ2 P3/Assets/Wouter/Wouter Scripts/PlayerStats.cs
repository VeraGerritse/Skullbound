using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {


    public float playerHealth;
    public float playerStamina;
    public  GameObject weapon;
    public  GameObject previousWeapon;

    public List<GameObject> viewmodelgear = new List<GameObject>();


    public bool playerBLocks;

    public void ChangeHealth(float amount)
    {
        playerHealth += amount;
    }


}
