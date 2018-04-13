using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour {
    public Canvas gameInterface;
    public Text canInteract;
    public bool inter;
    public Image Health_UI_Red;
    public Image Health_UI_Yellow;
    public Image Stamina_UI_Green;
    public Image Stamina_UI_Yellow;


    public List<Image> hearts = new List<Image>();
    public List<Image> stams = new List<Image>();

    public Image boost1;
    public Image boost2;
    public Image boost3;

    public Animator anim;

  

    public void Interact()
    {
        
        
        if (inter)
        {
            CanInteract();
        }
        else
        {
            CantInteract();
        }
    }

    public void UpdateHealth(float currentHealth,float maxHealth)
    {
        float procentage = currentHealth / maxHealth;
        Health_UI_Red.fillAmount = procentage;
        anim.SetTrigger("pop");

        //procentage lmao
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        float procentage = currentStamina / maxStamina;
        Stamina_UI_Green.fillAmount = procentage;
        
    }

    void CanInteract()
    {
        canInteract.gameObject.SetActive(true);
    }

    void CantInteract()
    {
        canInteract.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Health_UI_Red.fillAmount < Health_UI_Yellow.fillAmount)
        {
            Health_UI_Yellow.fillAmount -= Time.deltaTime * 2 * (Health_UI_Yellow.fillAmount - Health_UI_Red.fillAmount);
        }
        else
        {
            Health_UI_Yellow.fillAmount = Health_UI_Red.fillAmount;
        }

        if (Stamina_UI_Green.fillAmount < Stamina_UI_Yellow.fillAmount)
        {
            Stamina_UI_Yellow.fillAmount -= Time.deltaTime * 2 * (Stamina_UI_Yellow.fillAmount - Stamina_UI_Green.fillAmount);
        }
        else
        {
            Stamina_UI_Yellow.fillAmount = Stamina_UI_Green.fillAmount;
        }

    }
}
