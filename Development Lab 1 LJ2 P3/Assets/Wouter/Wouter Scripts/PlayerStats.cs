﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float playerMaxHealth;
    public float playerHealth;
    public float playerStamina;
    public float playerMaxStamina;
    public int potionCount;
    public GameObject weapon;
    public GameObject previousWeapon;
    public GameObject shield;
    public GameObject previousShield;

    public bool doStaminaBoost;
    public float boostDuration;
        

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
        if(amount < 0)
        {
            SoundManager.soundInstance.audiosources[Random.Range(0, 4)].Play();
        }
        
    }

    public void ChangeStamina(float amount)
    {
        playerStamina += amount;
        UIManager.instance.interfaceGame.UpdateStamina(playerStamina, playerMaxStamina);
        if(amount < 0)
        {
            UIManager.instance.interfaceGame.GetComponent<Animator>().SetTrigger("popS");
        }
        
    }

    public void ActivateStaminaBoost()
    {
        doStaminaBoost = true;
        boostDuration = 10;
    }

    private void Update()
    {
        if(doStaminaBoost && boostDuration > 0)
        {
            boostDuration -= Time.deltaTime;

            if (playerStamina < playerMaxStamina && !playerBLocks)
            {
                ChangeStamina(Time.deltaTime * 20);
            }
        }

        if (playerStamina < playerMaxStamina && !playerBLocks)
        {
            ChangeStamina(Time.deltaTime * 5);
        }
    }


}
