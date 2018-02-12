using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public RoomGen up;
    public RoomGen left;
    public RoomGen right;
    public RoomGen down;

    public bool leftDoor;
    public bool rightDoor;
    public bool upDoor;
    public bool downDoor;

    public GameObject myFloor;
    public bool done;
    public bool doneWalls;
    public int chance;

    public enum From { left, right, up, down, none }

    public void InstantiateFloor()
    {

        if (done)
        {
            return;
        }
        myFloor = Instantiate(DungeonGeneratorManager.instance.RandomRoom(), transform.position, Quaternion.identity);
        myFloor.transform.SetParent(gameObject.transform);
        done = true;
        int rand = 100;
        if (up != null)
        {
            rand = Random.Range(0, 100);
            if (rand < chance)
            {
                up.InstantiateFloor();
            }
        }
        if (down != null)
        {
            rand = Random.Range(0, 100);
            if (done || rand < chance)
            {
                down.InstantiateFloor();
            }
        }
        if (left != null)
        {
            rand = Random.Range(0, 100);
            if (done || rand < chance)
            {
                left.InstantiateFloor();
            }
        }
        if (right != null)
        {
            rand = Random.Range(0, 100);
            if (rand < chance)
            {   
                right.InstantiateFloor();
            }
        }
    }

    public void Doors()
    {
        doneWalls = true;
        if (left != null)
        {
            if (left.myFloor != null)
            {
                if (!left.doneWalls)
                {
                    leftDoor = true;
                    left.rightDoor = true;
                    left.Doors();
                }
            }
        }
        if (right != null)
        {
            if (right.myFloor != null)
            {
                if (!right.doneWalls)
                {
                    rightDoor = true;
                    right.leftDoor = true;
                    right.Doors();
                }
            }
        }
        if (up != null)
        {
            if (up.myFloor != null)
            {
                if (!up.doneWalls)
                {
                    upDoor = true;
                    up.downDoor = true;
                    up.Doors();
                }
            }
        }
        if (down != null)
        {
            if (down.myFloor != null)
            {
                if (!down.doneWalls)
                {
                    downDoor = true;
                    down.upDoor = true;
                    down.Doors();
                }
            }
        }
    }
}

