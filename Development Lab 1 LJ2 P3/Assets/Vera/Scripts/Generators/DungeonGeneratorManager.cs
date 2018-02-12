using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorManager : MonoBehaviour
{
    [Header("Grid size instantiation")]
    public int gridSize;
    public GameObject locations;
    public float roomSize;
    public List<RoomGen> possiblePlaces = new List<RoomGen>();
    public List<GameObject> floors = new List<GameObject>();
    public List<GameObject> bossRooms = new List<GameObject>();

    public static DungeonGeneratorManager instance;

    [Header("Chance")]
    public int chance;
    public int roomsNeeded;
    public int maxRooms;
    public int chanceOnDoor;

    [Header ("walls")]
    public List<GameObject> verticalWall = new List<GameObject>();
    public List<GameObject> horizontalWall = new List<GameObject>();
    public List<GameObject> myWalls = new List<GameObject>();
    bool walls;

    [Header("player")]
    public GameObject playerPreFab;
    public GameObject mapCamera;
    GameObject player;
    GameObject mapCam;


    [Header("BossRoom")]
    RoomGen lastRoom;
    bool done;

    [Header("MapBuilding")]
    public GameObject mapFloor;
    List<GameObject> allMapPieces = new List<GameObject>();
    public List<GameObject> mapWalls = new List<GameObject>();



    void Start()
    {
        if(maxRooms < roomsNeeded)
        {
            maxRooms = roomsNeeded + 1;
        }
        if (instance == null)
        {
            instance = this;
        }
        GenerateFloor();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            ResetDungeon();
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
                    possiblePlaces.Add(newSpace.GetComponent<RoomGen>());
                }
                xStart = corner * roomSize;
                zStart += roomSize;
            }
            MapManager.instance.ListLenght(possiblePlaces.Count);
            AssignNeigbours();
        }

    }

    void AssignNeigbours()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if (i - gridSize >= 0)
            {
                possiblePlaces[i].up = possiblePlaces[i - gridSize];
            }
            if (i + gridSize < possiblePlaces.Count)
            {
                possiblePlaces[i].down = possiblePlaces[i + gridSize];
            }
            if (i % gridSize != 0)
            {
                possiblePlaces[i].left = possiblePlaces[i - 1];
            }

            if ((i + 1) % gridSize != 0)
            {
                possiblePlaces[i].right = possiblePlaces[i + 1];
            }
        }
        GenerateFloorPlan();
    }

    void GenerateFloorPlan()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            possiblePlaces[i].chance = chance;
        }
        int startPoint = Random.Range(0, possiblePlaces.Count);
        possiblePlaces[startPoint].chance = 100;
        possiblePlaces[startPoint].InstantiateFloor();
        int totalRooms = 0;
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if (possiblePlaces[i].myFloor != null)
            {
                totalRooms++;
            }
        }
        if (totalRooms < roomsNeeded || totalRooms > maxRooms)
        {
            ResetDungeon();
        }
        PlacePlayer(startPoint);
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            possiblePlaces[i].done = false;
        }
        possiblePlaces[startPoint].Doors();
        MapFloor();
        BuildWalls();
        PlaceBossRoom(startPoint-1,startPoint+1,startPoint- gridSize, startPoint + gridSize);
    }

    public GameObject RandomRoom()
    {
        int rand = Random.Range(0, floors.Count);
        return floors[rand];
    }

    bool Neighbour(int current)
    {
        if ((possiblePlaces[current].myFloor != null))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ResetDungeon()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if (possiblePlaces[i].myFloor != null)
            {
                Destroy(possiblePlaces[i].myFloor);
                possiblePlaces[i].myFloor = null;
            }
            possiblePlaces[i].done = false;
            possiblePlaces[i].leftDoor = false;
            possiblePlaces[i].rightDoor = false;
            possiblePlaces[i].upDoor = false;
            possiblePlaces[i].downDoor = false;
            possiblePlaces[i].doneWalls = false;
        }
        for (int i = 0; i < myWalls.Count; i++)
        { 
            Destroy(myWalls[i]);
        }
        for (int i = 0; i < allMapPieces.Count; i++)
        {
            Destroy(allMapPieces[i]);
        }
        Destroy(player);
        walls = false;
        player = null;
        done = false;
        myWalls.Clear();
        GenerateFloorPlan();
    }

    GameObject PlaceWall(int index, Vector3 pos, bool vert)
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

    public void PlacePlayer(int i)
    {
        Vector3 loc = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y + 1, possiblePlaces[i].transform.position.z);
        if (player == null)
        {
            player = Instantiate(playerPreFab, loc, Quaternion.identity);
        }
        if(mapCam == null)
        {
            mapCam = Instantiate(mapCamera, loc, Quaternion.identity);
            mapCam.GetComponent<MapMovement>().player = player.transform;
        }
    }

    void BuildWalls()
    {
      if(!walls)
        {
            Vector3 wallPos;
            walls = true;
            GameObject newWall = null;
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                wallPos = new Vector3(possiblePlaces[i].transform.position.x + roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
                if (possiblePlaces[i].leftDoor && possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(0, wallPos, true);
                    GameObject newWallUgh = Instantiate(mapWalls[0], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                else if (possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(1, wallPos, true);
                    GameObject newWallUgh = Instantiate(mapWalls[1], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }               
                if(newWall != null)
                {
                    newWall.transform.SetParent(possiblePlaces[i].transform);
                    myWalls.Add(newWall);
                }
                newWall = null;

                wallPos = new Vector3(possiblePlaces[i].transform.position.x - roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
                if (possiblePlaces[i].rightDoor && possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(0, wallPos, true);
                    GameObject newWallUgh = Instantiate(mapWalls[0], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                else if (possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(1, wallPos, true);
                    GameObject newWallUgh = Instantiate(mapWalls[1], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                if (newWall != null)
                {
                    newWall.transform.SetParent(possiblePlaces[i].transform);
                    myWalls.Add(newWall);
                }
                newWall = null;

                wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z - roomSize / 2);
                if (possiblePlaces[i].upDoor && possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(0, wallPos, false);
                    GameObject newWallUgh = Instantiate(mapWalls[2], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                else if (possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(1, wallPos, false);
                    GameObject newWallUgh = Instantiate(mapWalls[3], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                if (newWall != null)
                {
                    newWall.transform.SetParent(possiblePlaces[i].transform);
                    myWalls.Add(newWall);
                }
                newWall = null;

                wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z + roomSize / 2);
                if (possiblePlaces[i].downDoor && possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(0, wallPos, false);
                    GameObject newWallUgh = Instantiate(mapWalls[2], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }
                else if (possiblePlaces[i].myFloor != null)
                {
                    newWall = PlaceWall(1, wallPos, false);
                    GameObject newWallUgh = Instantiate(mapWalls[3], wallPos, Quaternion.identity);
                    newWallUgh.transform.SetParent(MapManager.instance.allChambers[i].transform);
                    allMapPieces.Add(newWallUgh);
                }

                if (newWall != null)
                {
                    newWall.transform.SetParent(possiblePlaces[i].transform);
                    myWalls.Add(newWall);
                }
                newWall = null;
            }

        } 

    }

    void PlaceBossRoom(int up,int down,int left,int right)
    {
        if (!done)
        {
            List<int> availableBossRooms = new List<int>();
            availableBossRooms.Clear();
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                if (possiblePlaces[i].myFloor != null && i != up && i != down && i != left && i != right)
                {
                    availableBossRooms.Add(i);
                }
            }
            int newBossRoom = Random.Range(0, availableBossRooms.Count);
            int rand = Random.Range(0, bossRooms.Count);
            Destroy(possiblePlaces[availableBossRooms[newBossRoom]].myFloor);
            possiblePlaces[newBossRoom].myFloor = Instantiate(bossRooms[rand], possiblePlaces[availableBossRooms[newBossRoom]].transform.position, Quaternion.identity);
            done = true;
        }
    }

    void MapFloor()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if(possiblePlaces[i].myFloor != null)
            {
                GameObject newShitz = Instantiate(mapFloor, possiblePlaces[i].transform.position, Quaternion.identity);
                MapManager.instance.allChambers[i] = newShitz;
                allMapPieces.Add(newShitz);
            }
        }
    }
}
