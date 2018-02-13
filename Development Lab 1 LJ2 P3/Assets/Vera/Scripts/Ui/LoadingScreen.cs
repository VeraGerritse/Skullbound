using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public float levelsNeeded;
    public float currentLvl;
    float amountPerLvl;

    public Image loadBar;
    public Canvas loadingScreen;
    public Camera cam;

    private void Awake()
    {
        amountPerLvl = 1 / levelsNeeded;
    }

    public void Loading()
    {
        print("hoi");
        currentLvl += amountPerLvl;
        loadBar.fillAmount = currentLvl;
        if(currentLvl >= 1)
        {
            loadingScreen.enabled = false;
            cam.gameObject.SetActive(false);
        }
        else
        {
            loadingScreen.enabled = true;
            cam.gameObject.SetActive(true);
        }
    }

    public void ResetLoad()
    {
        currentLvl = 0;
        loadBar.fillAmount = currentLvl;
        loadingScreen.enabled = true;
        cam.gameObject.SetActive(true);
    }
}
