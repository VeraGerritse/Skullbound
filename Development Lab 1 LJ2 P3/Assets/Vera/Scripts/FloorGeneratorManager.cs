using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneratorManager : MonoBehaviour {
    public List<RandomFloorGenerator> possiblePlaces = new List<RandomFloorGenerator>();
    public List<GameObject> floors = new List<GameObject>();
    public List<GameObject> allWalls = new List<GameObject>();
    public GameObject playerPreFab;
    public GameObject player;
    public int changeOnRoom;
    public int minimumRooms;
    int randomRoom;

    [Header("Grid size instantiation")]
    public int gridSize;
    public GameObject locations;
    public float roomSize;

    [Header("walls")]
    public List<GameObject> verticalWall = new List<GameObject>();
    public List<GameObject> horizontalWall = new List<GameObject>();

    public static FloorGeneratorManager instance;

    void Start () {
        if(instance == null)
        {
            instance = this;
        }
        GenerateFloor();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetRooms();
        }
    }

    void GenerateFloor()
    {
        int corner = Mathf.RoundToInt(gridSize / 2);
        float xStart = corner * roomSize;
        float zStart = -corner * roomSize;
        if (gridSize != 0)
        {
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    GameObject newSpace = Instantiate(locations, new Vector3(xStart, 0, zStart), Quaternion.identity);
                    xStart -= roomSize;
                    possiblePlaces.Add(newSpace.GetComponent<RandomFloorGenerator>());
                }
                xStart = corner * roomSize;
                zStart += roomSize;
            }
            GenerateFloorPlan();
        }
    }

    void GenerateFloorPlan()
    {
        if (gridSize != 0)
        {
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                if (!possiblePlaces[i].done)
                {
                    if (Random.Range(0, 100) > changeOnRoom)
                    {
                        possiblePlaces[i].InstantiateRoom(floors[Random.Range(0, floors.Count)]);
                    }
                }
            }
            DestroyLittleIslands();

            bool inRoom = false;
            randomRoom = -1;
            while (!inRoom)
            {
                int i = Random.Range(0, possiblePlaces.Count);
                if (possiblePlaces[i].myRoom != null)
                {
                    inRoom = true;
                    randomRoom = i;
                }
            }
            print(randomRoom);
            DestroyIslands(randomRoom);
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                if (possiblePlaces[i].myRoom != null)
                {
                    if (!possiblePlaces[i].noIsland)
                    {
                        Destroy(possiblePlaces[i].myRoom);
                        possiblePlaces[i].KillChildren();
                        possiblePlaces[i].myRoom = null;
                    }
                }
            }
            //BuildWalls();
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                if(possiblePlaces[i].myRoom == null)
                {
                    possiblePlaces[i].KillChildren();
                }
            }
            DestroyLittleIslands();
            int amountRooms = 0;
            for (int i = 0; i < possiblePlaces.Count; i++)
            {

                if (possiblePlaces[i].myRoom != null)
                {
                    amountRooms++;
                }
            }
            bool inRoom2 = false;
            randomRoom = -1;
            while (!inRoom2)
            {
                int i = Random.Range(0, possiblePlaces.Count);
                if (possiblePlaces[i].myRoom != null)
                {
                    inRoom2 = true;
                    randomRoom = i;
                }
            }
            if (randomRoom != -1)
            {
                PlacePlayer(randomRoom); 
            }
 
            if (amountRooms < minimumRooms)
            {
                ResetRooms();
            }
        }
    }

    void DestroyIslands(int startRoom)
    {
        List<int> startList = new List<int>
        {
            startRoom
        };
        possiblePlaces[startRoom].noIsland = true;
        ListPlayIslands(startList);
    }

    void ListPlayIslands(List<int> indexList)
    {
        List<int> newList = new List<int>();
        newList.Clear();
            for (int i = 0; i < indexList.Count; i++)
            {
                
                if (i - gridSize >= 0)
                {
                    if (possiblePlaces[i - gridSize].myRoom != null && possiblePlaces[i - gridSize].noIsland == false)
                    {
                        possiblePlaces[i - gridSize].noIsland = true;
                        newList.Add(i - gridSize);
                    }
                }
                if (i % gridSize != 0)
                {
                    if (possiblePlaces[i - 1].myRoom != null && possiblePlaces[i - 1].noIsland == false)
                    {
                        possiblePlaces[i - 1].noIsland = true;
                        newList.Add(i - 1);
                    }
                }
                if ((i + 1) % gridSize != 0)
                {
                    if (possiblePlaces[i + 1].myRoom != null && possiblePlaces[i + 1].noIsland == false)
                    {
                        possiblePlaces[i + 1].noIsland = true;
                        newList.Add(i + 1);
                    }
                }
                if (i + gridSize < possiblePlaces.Count)
                {
                    if (possiblePlaces[i + gridSize].myRoom != null && possiblePlaces[i + gridSize].noIsland == false)
                    {
                        possiblePlaces[i + gridSize].noIsland = true;
                        newList.Add(i + gridSize);
                    }
                }

            }
        indexList.Clear();
        ListLoop(newList);
    }

    void ListLoop(List<int> ints)
    {
        if(ints.Count != 0)
        {
            ListPlayIslands(ints);
        }
    }

    GameObject PlaceWall(int index,Vector3 pos,bool vert)
    {
        if (vert)
        {
            return Instantiate(verticalWall[index], pos, Quaternion.identity);
        }
        else
        {
            return Instantiate(horizontalWall[index], pos, Quaternion.identity);
        }

    }

    bool Neighbour(int current,int now)
    {
        if((possiblePlaces[current].myRoom != null))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlacePlayer(int i)
    {
            Vector3 loc = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y + 1, possiblePlaces[i].transform.position.z);
        if(player == null)
        {
            player = Instantiate(playerPreFab, loc, Quaternion.identity);
        }

    }

    public void ResetRooms()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if(possiblePlaces[i].myRoom != null)
            {
                Destroy(possiblePlaces[i].myRoom);
            }
            possiblePlaces[i].myRoom = null;
            possiblePlaces[i].done = false;
            for (int p = 0; p < possiblePlaces[i].myWalls.Count; p++)
            {
                Destroy(possiblePlaces[i].myWalls[p]);
            }
            possiblePlaces[i].myWalls.Clear();
            possiblePlaces[i].noIsland = false;
        }
        Destroy(player);
        player = null;
        allWalls.Clear();
        GenerateFloorPlan();       
    }

    void BuildWalls()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            Vector3 wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z - roomSize / 2);
            GameObject newWall = null;
            if (i - gridSize >= 0)
            {
                if (Neighbour(i - gridSize, i) && possiblePlaces[i].myRoom != null)
                {
                    newWall = PlaceWall(0, wallPos, false);
                }
                else
                {
                    newWall = PlaceWall(1, wallPos, false);
                }
            }
            else if (possiblePlaces[i].myRoom != null)
            {
                newWall = PlaceWall(1, wallPos, false);
            }
            possiblePlaces[i].myWalls.Add(newWall);
            if (newWall != null)
            {
                newWall.transform.SetParent(possiblePlaces[i].gameObject.transform);
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z + roomSize / 2);
            if (i + gridSize < possiblePlaces.Count)
            {
                if (Neighbour(i + gridSize, i) && possiblePlaces[i].myRoom != null)
                {
                    newWall = PlaceWall(0, wallPos, false);
                }
                else
                {
                    newWall = PlaceWall(1, wallPos, false);
                }
            }
            else if (possiblePlaces[i].myRoom != null)
            {
                newWall = PlaceWall(1, wallPos, false);
            }
            possiblePlaces[i].myWalls.Add(newWall);
            if (newWall != null)
            {
                newWall.transform.SetParent(possiblePlaces[i].gameObject.transform);
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x + roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
            if (i % gridSize != 0)
            {
                if (Neighbour(i - 1, i) && possiblePlaces[i].myRoom != null)
                {
                    newWall = PlaceWall(0, wallPos, true);
                }
                else
                {
                    newWall = PlaceWall(1, wallPos, true);
                }
            }
            else if (possiblePlaces[i].myRoom != null)
            {
                newWall = PlaceWall(1, wallPos, true);
            }
            possiblePlaces[i].myWalls.Add(newWall);
            if (newWall != null)
            {
                newWall.transform.SetParent(possiblePlaces[i].gameObject.transform);
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x - roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
            if ((i + 1) % gridSize != 0)
            {
                if (Neighbour(i + 1, i) && possiblePlaces[i].myRoom != null)
                {
                    newWall = PlaceWall(0, wallPos, true);
                }
                else
                {
                    newWall = PlaceWall(1, wallPos, true);
                }
            }
            else if (possiblePlaces[i].myRoom != null)
            {
                newWall = PlaceWall(1, wallPos, true);
            }
            possiblePlaces[i].myWalls.Add(newWall);
            if (newWall != null)
            {
                newWall.transform.SetParent(possiblePlaces[i].gameObject.transform);
            }
        }
    }

    void DestroyLittleIslands()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if (possiblePlaces[i].myRoom != null)
            {
                bool iHaveNeighbours = false;
                if (i - gridSize >= 0)
                {
                    if (Neighbour(i - gridSize, i))
                    {
                        iHaveNeighbours = true;
                    }
                }

                if (i + gridSize < possiblePlaces.Count)
                {
                    if (Neighbour(i + gridSize, i))
                    {
                        iHaveNeighbours = true;
                    }
                }
                if (i % gridSize != 0)
                {
                    if (Neighbour(i - 1, i))
                    {
                        iHaveNeighbours = true;
                    }
                }

                if ((i + 1) % gridSize != 0)
                {
                    if (Neighbour(i + 1, i))
                    {
                        iHaveNeighbours = true;
                    }
                }

                if (!iHaveNeighbours)
                {
                    Destroy(possiblePlaces[i].myRoom);
                    possiblePlaces[i].KillChildren();
                    possiblePlaces[i].myRoom = null;
                }
            }
        }
    }
}
