using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

    public bool walk;
    public bool connectedToSomeThing;
    public Vector3 nodePosition;
    public Node(bool walkable, Vector3 worldPos,bool connected)
    {
        walk = walkable;
        nodePosition = worldPos;
        connectedToSomeThing = connected;
    }
}
