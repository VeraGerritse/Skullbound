using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float playerMaxHealth;
    public float playerHealth;
    public float playerStamina;
    public GameObject weapon;
    public GameObject previousWeapon;
    public GameObject shield;
    public GameObject previousShield;
        

    public List<GameObject> viewmodelgear = new List<GameObject>();
    public List<GameObject> viewmodelgearLeft = new List<GameObject>();


    public bool playerBLocks;

    private void Start()
    {
        playerHealth = playerMaxHealth;
        UIManager.instance.interfaceGame.UpdateHealth(playerHealth, playerMaxHealth);
    }
    public void ChangeHealth(float amount)
    {
        playerHealth += amount;
        UIManager.instance.interfaceGame.UpdateHealth(playerHealth, playerMaxHealth);
    }


}
