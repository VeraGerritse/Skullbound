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

        //procentage lmao
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
    }
}
