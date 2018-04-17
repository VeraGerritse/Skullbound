using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Pathfinding : Interactables {

    [Header("targets")]
    public Transform target;
    public Transform aI;
    public Transform skull;

    [Header("Stats")]
    public int speed;
    public float turnSpeed = 100;

    bool startUp;
    public bool atTarget;
    Node lastNode;

    public float exitRadius;


    private void StartUp()
    {
        aI = gameObject.transform;
        target = DungeonGeneratorManager.instance.player.transform;
        if(exitRadius == 0)
        {
            exitRadius = 3;
        }
    }

    public override void Interact()
    {
        if (!startUp && Grid.instance.ready && DungeonGeneratorManager.instance != null)
        {
            StartUp();
        }
        if (aI != null && target != null && Grid.instance.ready == true)
        {
            FindPath(aI.position, target.position);
        }

        if(atTarget)
        {
            if(Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position) >= exitRadius)
            {
                atTarget = false;
            }
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = Grid.instance.NodeFromWP(startPos);
        Node targetNode = Grid.instance.NodeFromWP(targetPos);

        if (!targetNode.walk)
        {
            targetNode = lastNode;
        }
        else
        {
            lastNode = targetNode;
        }
        //print(lastNode + "   " + targetNode);
        //if (targetNode == lastNode)
        //{
        //    RetracePath(startNode, targetNode);
        //    return;
        //}
        //lastNode = targetNode;
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
        if (!atTarget)
        {
            Vector3 start = transform.position;
            Vector3 targetLoc = new Vector3(NextLoc.nodePosition.x, transform.position.y, NextLoc.nodePosition.z);
            transform.position = Vector3.MoveTowards(transform.position, targetLoc, (speed / 10) * Time.deltaTime);
            if (start == transform.position)
            {
                targetLoc = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            }

            Vector3 relativePos = targetLoc - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, turnSpeed * Time.deltaTime);

            //Vector3 relPos = target.position - skull.position;
            //Quaternion lokRot = Quaternion.LookRotation(relPos);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lokRot, turnSpeed * Time.deltaTime);
        }

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        if(nodeA == null|| nodeB == null)
        {
            return 0;
        }
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

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector3 targetLoc = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);

            Vector3 relativePos = targetLoc - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
            atTarget = true;
        }
    }


}
