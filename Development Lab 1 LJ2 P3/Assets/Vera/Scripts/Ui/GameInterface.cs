using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour {
    public Canvas gameInterface;
    public Text canInteract;
    public bool inter;
    public Image Health_UI;

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
        Health_UI.fillAmount = procentage;
    }

    void CanInteract()
    {
        canInteract.gameObject.SetActive(true);
    }

    void CantInteract()
    {
        canInteract.gameObject.SetActive(false);
    }
}
