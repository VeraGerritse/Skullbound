using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    public GameInterface interfaceGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
