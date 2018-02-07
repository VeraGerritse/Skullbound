using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {
    public bool done;
    public bool start;
    public GameObject myRoom;
    public List<GameObject> myWalls = new List<GameObject>();



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
        print("i killed my child!!!");
        Transform parent = GetComponent<Transform>();
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
