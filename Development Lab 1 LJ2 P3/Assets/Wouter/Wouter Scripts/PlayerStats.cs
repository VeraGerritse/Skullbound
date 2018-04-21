using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float playerMaxHealth;
    public float playerHealth;
    public float playerStamina;
    public float playerMaxStamina;
    public int potionCount;
    public int boostCount;
    public float staminaRegenerationDelay;

    public Animator animator;
    
    public GameObject weapon;
    public GameObject previousWeapon;
    public GameObject shield;
    public GameObject previousShield;

    public bool doStaminaBoost;
    public float boostDuration;

    public static PlayerStats instance;

    public List<GameObject> viewmodelgear = new List<GameObject>();
    public List<GameObject> viewmodelgearLeft = new List<GameObject>();


    public bool playerBLocks;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        playerHealth = playerMaxHealth;
        UIManager.instance.interfaceGame.UpdateHealth(playerHealth, playerMaxHealth);
        GameObject firstWeapon = Instantiate(weapon, new Vector3(100, -100, 100), Quaternion.identity);
        firstWeapon.SetActive(false);
        weapon = firstWeapon;
        GameObject firstShield = Instantiate(shield, new Vector3(100, -100, 100), Quaternion.identity);
        firstShield.SetActive(false);
        shield = firstShield;
    }



    public void ChangeHealth(float amount)
    {
        playerHealth += amount;
        UIManager.instance.interfaceGame.UpdateHealth(playerHealth, playerMaxHealth);
        if(amount < 0)
        {
            SoundManager.soundInstance.audiosources[Random.Range(0, 4)].Play();
        }
        if(playerHealth <= 0)
        {
            Die();
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
        boostCount--;
        animator.ResetTrigger("Inject");
        PotionUI.instance.UpdateSyringe(boostCount);
    }

    private void Update()
    {
        if(doStaminaBoost && boostDuration > 0)
        {
            UIManager.instance.interfaceGame.GetComponent<Animator>().SetTrigger("popS");
            boostDuration -= Time.deltaTime;

            if (playerStamina < playerMaxStamina && !playerBLocks)
            {
                ChangeStamina(Time.deltaTime * 40);
            }
        }

        if (playerStamina < playerMaxStamina && !playerBLocks && staminaRegenerationDelay <= 0)
        {
            ChangeStamina(Time.deltaTime * 20);
        }

        if(staminaRegenerationDelay > 0 )
        {
            staminaRegenerationDelay -= Time.deltaTime;
        }
    }

    public void SaveWeapons()
    {
        if(weapon != null)
        {
            weapon.transform.SetParent(null);
            DontDestroyOnLoad(weapon);
        }
        if(shield != null)
        {
            shield.transform.SetParent(null);
            DontDestroyOnLoad(shield);
        }

    }

    public void LoadWeapons()
    {
        if(weapon != null)
        {
            GameObject tempWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            Destroy(weapon);
            weapon = tempWeapon;
        }
        if(shield != null)
        {
            GameObject tempShield = Instantiate(shield, transform.position, Quaternion.identity);
            Destroy(shield);
            shield = tempShield;
        }
    }

    public void Die()
    {
        UIManager.instance.interfaceGame.anim.SetBool("Death", true);
        animator.SetTrigger("Die");
        animator.SetBool("Died", true);
        StartCoroutine(GameManager.instance.Die());
        

    }
}
