using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneratorManager : MonoBehaviour {
    public List<RandomFloorGenerator> possiblePlaces = new List<RandomFloorGenerator>();
    public List<GameObject> floors = new List<GameObject>();
    public List<GameObject> allWalls = new List<GameObject>();
    public GameObject Player;
    public int changeOnRoom;
    public int minimumRooms;
    int roomsDone;
    bool done;
    bool alive;

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
        if (Input.GetKey(KeyCode.P))
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

            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                if (possiblePlaces[i].myRoom != null)
                {
                    bool iHaveNeighbours = false;

                    Vector3 wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z - roomSize / 2);
                    GameObject newWall = null;
                    if (i - gridSize >= 0)
                    {
                        if (Neighbour(i - gridSize, i))
                        {
                            iHaveNeighbours = true;
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
                        if (Neighbour(i + gridSize, i))
                        {
                            iHaveNeighbours = true;
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
                        if (Neighbour(i - 1, i))
                        {
                            iHaveNeighbours = true;
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
                        if (Neighbour(i + 1, i))
                        {
                            iHaveNeighbours = true;
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

                    if (!iHaveNeighbours)
                    {
                        Destroy(possiblePlaces[i].myRoom);
                        possiblePlaces[i].KillChildren();
                        possiblePlaces[i].myRoom = null;
                    }
                }
            }
            int amountRooms = 0;
            for (int i = 0; i < possiblePlaces.Count; i++)
            {

                if (possiblePlaces[i].myRoom != null)
                {
                    amountRooms++;
                }
            }

            if (amountRooms < minimumRooms)
            {
                ResetRooms();
            }
            if (!alive)
            {
                PlacePlayer();
                alive = true;
            }
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
    public void PlacePlayer()
    {
        bool inRoom = false;
        while (!inRoom)
        {
            int i = Random.Range(0, possiblePlaces.Count);
            if(possiblePlaces[i].myRoom != null)
            {
                Vector3 loc = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y + 1, possiblePlaces[i].transform.position.z);
                Instantiate(Player, loc, Quaternion.identity);
                inRoom = true;
            }
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
        }

        allWalls.Clear();
        GenerateFloorPlan();       
    }
}
