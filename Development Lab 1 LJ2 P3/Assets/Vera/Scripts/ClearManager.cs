using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour {

    public static ClearManager instance;
    public List<Door> doors = new List<Door>();
    public bool allCleared;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        print(instance);
        allCleared = true;
    }

    public void EnterRoom()
    {
        allCleared = false;
        print("test");
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].CloseDoors();
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
}
