using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameObject player;
    public bool paused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        paused = true;
        StartCoroutine(StartShiz());
    }
    

    private void Update()
    {
        UpDateThese();
        if (Input.GetKeyDown(KeyCode.F))
        {
            paused = false;
        }
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
