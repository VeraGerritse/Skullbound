using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivities : MonoBehaviour {

    //public List<GameObject> rBodys = new List<GameObject>();
    public List<Interactables> interactable = new List<Interactables>();
    public List<GameObject> enemys = new List<GameObject>();
    public List<Transform> spawnLoc = new List<Transform>();
    public List<CombatAi> enemysAlive = new List<CombatAi>();
    public bool bossRoom;
    bool roomDone;
    public RoomGen myRoom;
    public int SpawnRate;
    bool inRoom;

    private void Start()
    {
        myRoom = GetComponentInParent<RoomGen>();
        myRoom.myActivities = this;
    }


    public void EnableRigidBodys()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void DisableRigidBodys()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void StartInteracting()
    {
        if(spawnLoc.Count != 0 && enemys.Count != 0)
        {
            SpawnSkeletons();           
        }
        EnemyKilled(null);
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = true;
        }
    }

    public void StopInteracting()
    {
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (other.gameObject.tag == "Player" && inRoom)
        {
            inRoom = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && !inRoom && DungeonGeneratorManager.instance.ready)
        {
            ClearManager.instance.EnterRoom();
            StartInteracting();
            inRoom = true;
            DungeonGeneratorManager.instance.EnterRoom(myRoom);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRoom = false;
            StopInteracting();
        }
    }

    public void SpawnSkeletons()
    {
        int spawnChance = 0;
        for (int i = 0; i < spawnLoc.Count; i++)
        {
            spawnChance = Random.Range(0, 100);
            if (bossRoom)
            {
                spawnChance = 0;
            }
            if (spawnChance < SpawnRate)
            {
                GameObject newEnemy = Instantiate(enemys[Random.Range(0, enemys.Count)], spawnLoc[i].position, Quaternion.identity);
                newEnemy.GetComponent<CombatAi>().myRoom = this;
                newEnemy.GetComponent<Pathfinding>().IsAwake = true;
                enemysAlive.Add(newEnemy.GetComponent<CombatAi>());
            }
        }
    }

    public void EnemyKilled(CombatAi deadEnemy)
    {
        print("killed");
        if (enemysAlive.Count == 0)
        {
            print("hoi");
            ClearManager.instance.ExitRoom();
            return;
        }
        for (int i = 0; i < enemysAlive.Count; i++)
        {
            if(enemysAlive[i] == deadEnemy)
            {
                enemysAlive.RemoveAt(i);
                if(enemysAlive.Count == 0)
                {
                    ClearManager.instance.ExitRoom();
                    return;
                }
            }
        }

    }
}
