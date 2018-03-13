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

    [Header("Doors")]
    // oneven = vert, even = hor
    public List<GameObject> Doors = new List<GameObject>();
    List<GameObject> allDoors = new List<GameObject>();

    [Header("player")]
    public GameObject playerPreFab;
    public GameObject mapCamera;
    public GameObject player;
    GameObject mapCam;


    [Header("BossRoom")]
    RoomGen lastRoom;
    bool done;
    public Transform bossRoomLocTestForPathFinder;

    [Header("Start Room")]
    public List<GameObject> startingRooms = new List<GameObject>();
    bool ughie;

    [Header("MapBuilding")]
    public GameObject mapFloor;
    List<GameObject> allMapPieces = new List<GameObject>();
    public List<GameObject> mapWalls = new List<GameObject>();

    [Header("Rooms")]
    RoomGen currentRoom;
    RoomGen LastRoom;

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
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            ResetDungeonOnRequest();
        }
    }

    public void GenerateFloor()
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
        currentRoom = possiblePlaces[startPoint];

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
            return;
        }
        PlacePlayer(startPoint);
        PlaceStartRoom(startPoint);

        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            possiblePlaces[i].done = false;
        }

        possiblePlaces[startPoint].Doors();;
        bool reset = PlaceBossRoom(startPoint - 1, startPoint + 1, startPoint - gridSize, startPoint + gridSize, startPoint);
        if (reset)
        {
            print("resetting");
            return;
        }

        MapFloor();
        BuildWalls();
        PlaceDoors();
        StartCoroutine(StartPathfinder());
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
        ughie = false;
        if(mapCam != null)
        {
            mapCam.GetComponent<MapMovement>().player = null;
        }
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
        Vector3 loc = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y + 2, possiblePlaces[i].transform.position.z);
        if (player == null)
        {
            player = Instantiate(playerPreFab, loc, Quaternion.identity);
        }
        if(mapCam == null)
        {
            mapCam = Instantiate(mapCamera, loc, Quaternion.identity);
            mapCam.GetComponent<MapMovement>().player = player.transform;
        }
        else if(mapCam!= null)
        {
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

    void PlaceStartRoom(int startRoom)
    {
        if (!ughie)
        {
            Destroy(possiblePlaces[startRoom].myFloor);
            possiblePlaces[startRoom].myFloor = Instantiate(startingRooms[0], possiblePlaces[startRoom].transform.position, Quaternion.identity);
            possiblePlaces[startRoom].myFloor.transform.SetParent(possiblePlaces[startRoom].gameObject.transform);
            ughie = true;
        }
    }

    bool PlaceBossRoom(int up,int down,int left,int right, int startPoint)
    {
        bool reset = false;
        if (!done)
        {
            List<int> availableBossRooms = new List<int>();
            availableBossRooms.Clear();
            for (int i = 0; i < possiblePlaces.Count; i++)
            {
                bool oneWay = false;
                int doorsAvailable = 0;
                if (possiblePlaces[i].leftDoor)
                {
                    doorsAvailable++;
                }
                if (possiblePlaces[i].rightDoor)
                {
                    doorsAvailable++;
                }
                if (possiblePlaces[i].upDoor)
                {
                    doorsAvailable++;
                }
                if (possiblePlaces[i].downDoor)
                {
                    doorsAvailable++;
                }

                if(doorsAvailable == 1)
                {
                    oneWay = true;
                }
                if (possiblePlaces[i].myFloor != null && i != up && i != down && i != left && i != right && i != startPoint && oneWay)
                {
                    availableBossRooms.Add(i);
                }
            }
            
            if(availableBossRooms.Count == 0)
            {
                ResetDungeon();
                return true;
            }
            int newBossRoom = Random.Range(0, availableBossRooms.Count);
            int rand = Random.Range(0, bossRooms.Count);
            Destroy(possiblePlaces[availableBossRooms[newBossRoom]].myFloor);
            possiblePlaces[newBossRoom].myFloor = Instantiate(bossRooms[rand], possiblePlaces[availableBossRooms[newBossRoom]].transform.position, Quaternion.identity);
            possiblePlaces[newBossRoom].myFloor.transform.SetParent(possiblePlaces[newBossRoom].gameObject.transform);
            done = true;
            bossRoomLocTestForPathFinder = possiblePlaces[newBossRoom].myFloor.transform;
        }
        return reset;
    }

    void PlaceDoors()
    {
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            Vector3 wallPos;
            int randomDoor = Random.Range(0, Doors.Count);
            if(randomDoor % 2 != 0 && randomDoor != 0)
            {
                randomDoor--;
            }
            wallPos = new Vector3(possiblePlaces[i].transform.position.x + roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
            if (possiblePlaces[i].myFloor != null && possiblePlaces[i].leftDoor && !possiblePlaces[i].leftD)
            {
                possiblePlaces[i].leftD = true;
                possiblePlaces[i].left.rightD = true;
                allDoors.Add(Instantiate(Doors[randomDoor],wallPos,Quaternion.identity));
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x - roomSize / 2, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z);
            if(possiblePlaces[i].myFloor!= null && possiblePlaces[i].rightDoor && !possiblePlaces[i].rightD)
            {
                possiblePlaces[i].rightD = true;
                possiblePlaces[i].right.leftD = true;
                allDoors.Add(Instantiate(Doors[randomDoor], wallPos, Quaternion.identity));
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z - roomSize / 2);
            if (possiblePlaces[i].myFloor != null && possiblePlaces[i].upDoor && !possiblePlaces[i].upD)
            {
                possiblePlaces[i].upD = true;
                possiblePlaces[i].up.downD = true;
                allDoors.Add(Instantiate(Doors[randomDoor + 1], wallPos, Quaternion.identity));
            }

            wallPos = new Vector3(possiblePlaces[i].transform.position.x, possiblePlaces[i].transform.position.y, possiblePlaces[i].transform.position.z + roomSize / 2);
            if (possiblePlaces[i].myFloor != null && possiblePlaces[i].downDoor && !possiblePlaces[i].downD)
            {
                possiblePlaces[i].downD = true;
                possiblePlaces[i].down.upD = true;
                allDoors.Add(Instantiate(Doors[randomDoor + 1], wallPos, Quaternion.identity));
            }
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

    void ResetDungeonOnRequest()
    {
        Grid.instance.ResetGrid();
        ResetDungeon();
    }

    public void EnterRoom(RoomGen entering)
    {
        if (Grid.instance.ready)
        {
            if (currentRoom != null)
            {
                bool firstChamber = false;
                if(lastRoom == null)
                {
                    firstChamber = true;
                }
                lastRoom = currentRoom;
                currentRoom = entering;
                if (!firstChamber)
                {
                    if (lastRoom.left != currentRoom && lastRoom.left != null && lastRoom.left.myActivities != null)
                    {
                        lastRoom.left.myActivities.DisableRigidBodys();
                    }
                    if (lastRoom.right != currentRoom && lastRoom.right != null && lastRoom.right.myActivities != null)
                    {
                        lastRoom.right.myActivities.DisableRigidBodys();
                    }
                    if (lastRoom.up != currentRoom && lastRoom.up != null && lastRoom.up.myActivities != null)
                    {
                        lastRoom.up.myActivities.DisableRigidBodys();
                    }
                    if (lastRoom.down != currentRoom && lastRoom.down != null && lastRoom.down.myActivities != null)
                    {
                        lastRoom.down.myActivities.DisableRigidBodys();
                    }
                }
                if (currentRoom.up != null && currentRoom.up.myActivities != null)
                {
                    currentRoom.up.myActivities.EnableRigidBodys();
                }
                if (currentRoom.down != null && currentRoom.down.myActivities != null)
                {
                    currentRoom.down.myActivities.EnableRigidBodys();
                }
                if (currentRoom.left != null && currentRoom.left.myActivities != null)
                {
                    currentRoom.left.myActivities.EnableRigidBodys();
                }
                if (currentRoom.right != null && currentRoom.right.myActivities != null)
                {
                    currentRoom.right.myActivities.EnableRigidBodys();
                }
            }
        }
    }

    void AssignThings()
    {
        InteractManager.instance.actions = player.GetComponentInChildren<PlayerActions>();
    }

    IEnumerator StartPathfinder()
    {
        yield return new WaitForSeconds(0.1f);
        Grid.instance.GridSize(roomSize, gridSize);
        AssignThings();

        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < possiblePlaces.Count; i++)
        {
            if(possiblePlaces[i].myActivities!= null)
            {
                possiblePlaces[i].myActivities.DisableRigidBodys();
            }
        }
        EnterRoom(currentRoom);
    }
}
