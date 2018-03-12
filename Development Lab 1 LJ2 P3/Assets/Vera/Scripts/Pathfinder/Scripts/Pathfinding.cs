using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Pathfinding : Interactables {

    [Header("targets")]
    public Transform target;
    public Transform aI;

    [Header("Stats")]
    public int speed;
    public float turnSpeed = 20;

    bool startUp;

    Node lastNode;


    private void StartUp()
    {
        aI = gameObject.transform;
        target = DungeonGeneratorManager.instance.player.transform;
    }

    public override void Interact()
    {
        if (!startUp && Grid.instance.ready)
        {
            StartUp();
        }
        if (aI != null && target != null && Grid.instance.ready == true)
        {
            FindPath(aI.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = Grid.instance.NodeFromWP(startPos);
        Node targetNode = Grid.instance.NodeFromWP(targetPos);


        List<Node> openSet = new List<Node>();
        List<Node> ClosedSet = new List<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            ClosedSet.Add(currentNode);
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in Grid.instance.GetNeighbours(currentNode))
            {
                if(!neighbour.walk || !neighbour.connectedToSomeThing || ClosedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCost < neighbour.gCost || !openSet.Contains(neighbour)) 
                {
                    neighbour.gCost = newMovementCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        Grid.instance.path = path;
        if (path.Count != 0)
        {
            Move(path[0]);
        }
    }

    void Move(Node NextLoc)
    {
        Vector3 targetLoc = new Vector3(NextLoc.nodePosition.x,  transform.position.y, NextLoc.nodePosition.z);

        transform.position = Vector3.MoveTowards(transform.position, targetLoc, (speed / 10) * Time.deltaTime );
        transform.LookAt(targetLoc);
    }



    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
