using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour {

    public static ClearManager instance;
    public List<Door> doors = new List<Door>();
    public bool allCleared;
    public bool floorComplete;

    private void Awake()
    {
        doors.Clear();
        if(instance == null)
        {
            instance = this;
        }
        allCleared = true;

        floorComplete = false;
    }

    public void EnterRoom()
    {
        allCleared = false;
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].CloseDoors();
        }
    }
    private void Update()
    {
        if (floorComplete)
        {
            if (Input.GetButtonDown("Submit"))
            {
                CompleteFloor();
            }
        }
    }

    public void ExitRoom()
    {
        allCleared = true;
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].OpenDoor();
        }
    }

    public void CompleteFloor()
    {
        floorComplete = true;
        UIManager.instance.interfaceGame.endFloor.enabled = true;
    }

    public void NextFloor()
    {
        DungeonGeneratorManager.instance.Player();
        PlayerStats.instance.SaveWeapons();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
