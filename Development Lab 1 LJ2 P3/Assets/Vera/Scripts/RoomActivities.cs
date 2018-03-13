using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivities : MonoBehaviour {

    //public List<GameObject> rBodys = new List<GameObject>();
    public List<Interactables> interactable = new List<Interactables>();

    public RoomGen myRoom;

    private void Start()
    {
        myRoom = GetComponentInParent<RoomGen>();
        myRoom.myActivities = this;
    }


    public void EnableRigidBodys()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void DisableRigidBodys()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void StartInteracting()
    {
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = true;
        }
    }

    public void StopInteracting()
    {
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartInteracting();
            if (Grid.instance.ready)
            {
                print(myRoom + "   myroom  " + myRoom.myActivities);
            }
            DungeonGeneratorManager.instance.EnterRoom(myRoom);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopInteracting();
        }
    }
}
