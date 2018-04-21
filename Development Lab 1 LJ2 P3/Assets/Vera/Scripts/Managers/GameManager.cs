using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameObject player;
    public Grid grid;
    public bool paused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        paused = true;
        print("awake");
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
        yield return new WaitForSeconds(1f);
        print("startNow");
        OrderOfThings();
    }
    void OrderOfThings()
    {
        if (DungeonGeneratorManager.instance != null)
        {
            DungeonGeneratorManager.instance.GenerateFloor();
        }
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        Destroy(DungeonGeneratorManager.instance.player);
        SceneManager.LoadScene("Main_Menu_Entry");
    }
}
