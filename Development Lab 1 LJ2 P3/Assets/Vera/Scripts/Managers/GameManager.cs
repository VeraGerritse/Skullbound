﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        StartCoroutine(StartShiz());
    }

    private void Update()
    {
        UpDateThese();
    }

    void UpDateThese()
    {
        if(UIManager.instance.interfaceGame != null)
        {
            UIManager.instance.interfaceGame.Interact();
        }

    }

    IEnumerator StartShiz()
    {
        yield return new WaitForSeconds(0.1f);
        OrderOfThings();
    }
    void OrderOfThings()
    {
        if (DungeonGeneratorManager.instance != null)
        {
            DungeonGeneratorManager.instance.GenerateFloor();
        }
    }
}
