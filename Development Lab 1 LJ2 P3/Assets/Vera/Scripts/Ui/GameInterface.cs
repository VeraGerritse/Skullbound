using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour {
    public Canvas gameInterface;
    public Text canInteract;
    public bool inter;

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

    void CanInteract()
    {
        canInteract.gameObject.SetActive(true);
    }

    void CantInteract()
    {
        canInteract.gameObject.SetActive(false);
    }
}
