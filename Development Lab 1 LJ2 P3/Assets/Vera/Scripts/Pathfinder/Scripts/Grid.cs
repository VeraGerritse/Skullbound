using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public LayerMask unwalkableArea;
    public LayerMask noArea;
    public Vector2 gridSize;
    public float nodeRadius;

    public bool ready;

    public bool onlyDisplayPath;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public Grid instance;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void GridSize(float roomSize, float grid)
    {
        gridSize.x = roomSize * grid;
        gridSize.y = roomSize * grid;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottemLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottemLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableArea));
                bool inArea = (Physics.CheckSphere(worldPoint, nodeRadius));
                grid[i, y] = new Node(walkable, worldPoint,inArea,i,y);
            }
        }
        ready = true;
    }

    public Node NodeFromWP(Vector3 WorldPos)
    {
        float percentX = (WorldPos.x + gridSize.x / 2) / gridSize.x;
        float percentY = (WorldPos.z + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
        if (onlyDisplayPath)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.nodePosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    if (n.connectedToSomeThing)
                    {
                        Gizmos.color = (n.walk) ? Color.white : Color.red;
                        if (path != null)
                        {
                            if (path.Contains(n))
                            {
                                Gizmos.color = Color.black;
                            }
                        }
                        Gizmos.DrawCube(n.nodePosition, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }

        }
        return neighbours;
    }

    public void ResetGrid()
    {
        ready = false;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
            CreateGrid();
        }
    }
}
