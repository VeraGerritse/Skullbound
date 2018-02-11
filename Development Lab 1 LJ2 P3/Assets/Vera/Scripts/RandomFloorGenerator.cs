using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {
    public bool done;
    public bool start;
    public GameObject myRoom;
    public List<GameObject> myWalls = new List<GameObject>();

    public bool noIsland;



    public void RoomSetup()
    {
        done = true;
    }

    public void InstantiateRoom(GameObject room)
    {
        myRoom = Instantiate(room, transform.position, Quaternion.identity);
        RoomSetup();
    }

    public void KillChildren()
    {
        Transform parent = GetComponent<Transform>();
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
