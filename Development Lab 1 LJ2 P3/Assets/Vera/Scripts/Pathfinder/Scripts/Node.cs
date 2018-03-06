using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{

    public bool walk;
    public bool connectedToSomeThing;
    public Vector3 nodePosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkable, Vector3 worldPos,bool connected, int _gridX, int _gridY)
    {
        walk = walkable;
        nodePosition = worldPos;
        connectedToSomeThing = connected;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return HeapIndex;
        }
        set
        {
            HeapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;

    }
}
