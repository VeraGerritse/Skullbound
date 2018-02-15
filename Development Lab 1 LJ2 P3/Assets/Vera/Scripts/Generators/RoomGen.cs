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

    public int chanceDoor = 75;

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
        int rand = Random.Range(0,100);
        doneWalls = true;
        if (left != null)
        {
            if (left.myFloor != null)
            {
                if (!left.doneWalls || rand <= chanceDoor)
                {
                    leftDoor = true;
                    left.rightDoor = true;
                    left.doneWalls = true;
                }
            }
        }
        rand = Random.Range(0, 100);
        if (right != null)
        {
            if (right.myFloor != null)
            {
                if (!right.doneWalls || rand <= chanceDoor)
                {
                    rightDoor = true;
                    right.leftDoor = true;
                    right.doneWalls = true;
                }
            }
        }
        rand = Random.Range(0, 100);
        if (up != null)
        {
            if (up.myFloor != null)
            {
                if (!up.doneWalls || rand <= chanceDoor)
                {
                    upDoor = true;
                    up.downDoor = true;
                    up.doneWalls = true;
                }
            }
        }
        rand = Random.Range(0, 100);
        if (down != null)
        {
            if (down.myFloor != null)
            {
                if (!down.doneWalls || rand <= chanceDoor)
                {
                    downDoor = true;
                    down.upDoor = true;
                    down.doneWalls = true;
                }
            }
        }
        Doors2();
    }

    void Doors2()
    {
        List<int> sides = new List<int>()
        {
            0,1,2,3
        };

        while(sides.Count != 0)
        {
            int rand = Random.Range(0, sides.Count);
            int next = sides[rand];
            sides.RemoveAt(rand);

            if (left != null && next == 0)
            {
                if (left.myFloor != null)
                {
                    if (leftDoor && !left.done)
                    {
                        left.done = true;
                        left.Doors();
                    }
                }
            }
            if (right != null && next == 1)
            {
                if (right.myFloor != null)
                {
                    if (rightDoor && !right.done)
                    {
                        right.done = true;
                        right.Doors();
                    }
                }
            }
            if (up != null && next == 2)
            {
                if (up.myFloor != null)
                {
                    if (upDoor && !up.done)
                    {
                        up.done = true;
                        up.Doors();
                    }
                }
            }
            if (down != null && next == 3)
            {
                if (down.myFloor != null)
                {
                    if (downDoor && !down.done)
                    {
                        down.done = true;
                        down.Doors();
                    }
                }
            }
        }


        
        done = true;
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

